DROP TABLE IF EXISTS public.mt_streams CASCADE;
CREATE TABLE public.mt_streams (
    id                  uuid CONSTRAINT pk_mt_streams PRIMARY KEY,
    type                varchar NULL,
    version             integer NOT NULL,
    timestamp           timestamptz default (now()) NOT NULL,
    snapshot            jsonb ,
    snapshot_version    integer ,
    created             timestamptz default (now()) NOT NULL,
    tenant_id           varchar DEFAULT '*DEFAULT*'
);
COMMENT ON TABLE public.mt_streams IS 'origin:Marten.IDocumentStore, Marten, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null';
DROP TABLE IF EXISTS public.mt_events CASCADE;
CREATE TABLE public.mt_events (
    seq_id            bigint CONSTRAINT pk_mt_events PRIMARY KEY,
    id                uuid NOT NULL,
    stream_id         uuid REFERENCES public.mt_streams ON DELETE CASCADE,
    version           integer NOT NULL,
    data              jsonb NOT NULL,
    type              varchar(500) NOT NULL,
    timestamp         timestamptz default (now()) NOT NULL,
    tenant_id         varchar DEFAULT '*DEFAULT*',
    mt_dotnet_type    varchar NULL,
    CONSTRAINT pk_mt_events_stream_and_version UNIQUE(stream_id, version),
    CONSTRAINT pk_mt_events_id_unique UNIQUE(id)
);
COMMENT ON TABLE public.mt_events IS 'origin:Marten.IDocumentStore, Marten, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null';
DROP TABLE IF EXISTS public.mt_event_progression CASCADE;
CREATE TABLE public.mt_event_progression (
    name           varchar CONSTRAINT pk_mt_event_progression PRIMARY KEY,
    last_seq_id    bigint NULL
);
COMMENT ON TABLE public.mt_event_progression IS 'origin:Marten.IDocumentStore, Marten, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null';
CREATE SEQUENCE public.mt_events_sequence;
ALTER SEQUENCE public.mt_events_sequence OWNED BY public.mt_events.seq_id;

CREATE OR REPLACE FUNCTION public.mt_append_event(stream uuid, stream_type varchar, tenantid varchar, event_ids uuid[], event_types varchar[], dotnet_types varchar[], bodies jsonb[]) RETURNS int[] AS $$
DECLARE
	event_version int;
	event_type varchar;
	event_id uuid;
	body jsonb;
	index int;
	seq int;
    actual_tenant varchar;
	return_value int[];
BEGIN
	select version into event_version from public.mt_streams where id = stream;
	if event_version IS NULL then
		event_version = 0;
		insert into public.mt_streams (id, type, version, timestamp, tenant_id) values (stream, stream_type, 0, now(), tenantid);
    else
        if tenantid IS NOT NULL then
            select tenant_id into actual_tenant from public.mt_streams where id = stream;
            if actual_tenant != tenantid then
                RAISE EXCEPTION 'Marten: The tenantid does not match the existing stream';
            end if;
        end if;
	end if;

	index := 1;
	return_value := ARRAY[event_version + array_length(event_ids, 1)];

	foreach event_id in ARRAY event_ids
	loop
	    seq := nextval('public.mt_events_sequence');
		return_value := array_append(return_value, seq);

	    event_version := event_version + 1;
		event_type = event_types[index];
		body = bodies[index];

		insert into public.mt_events
			(seq_id, id, stream_id, version, data, type, tenant_id, mt_dotnet_type)
		values
			(seq, event_id, stream, event_version, body, event_type, tenantid, dotnet_types[index]);

		index := index + 1;
	end loop;

	update public.mt_streams set version = event_version, timestamp = now() where id = stream;

	return return_value;
END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION public.mt_mark_event_progression(name varchar, last_encountered bigint) RETURNS VOID LANGUAGE plpgsql AS
$function$
BEGIN
INSERT INTO public.mt_event_progression (name, last_seq_id) VALUES (name, last_encountered)
  ON CONFLICT ON CONSTRAINT pk_mt_event_progression
  DO UPDATE SET last_seq_id = last_encountered;

END;

$function$;