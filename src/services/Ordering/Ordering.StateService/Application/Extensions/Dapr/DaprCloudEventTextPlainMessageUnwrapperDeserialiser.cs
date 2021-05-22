using GreenPipes;
using MassTransit;
using MassTransit.Serialization;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;


namespace Ordering.StateService.Application.Extensions.Dapr
{
    public partial class DaprCloudEventTextPlainMessageUnwrapperDeserialiser : IMessageDeserializer
    {
        private const string MessageSourceType = "com.dapr.event.sent";

        public ContentType ContentType => new ContentType("text/plain");

        public void Probe(ProbeContext context)
        {
            var scope = context.CreateScope("darpCloudEventTextPlain");
            scope.Add("contentType", ContentType);
        }

        public ConsumeContext Deserialize(ReceiveContext receiveContext)
        {
            try
            {
                var messageEncoding = GetMessageEncoding(receiveContext);
                CloudEventMessageEnvelope cloudEventMessageEnvelope;

                using var body = receiveContext.GetBodyStream();
               
                using var reader = new StreamReader(body, messageEncoding, false, 1024, true);
                using (var jsonReader = new JsonTextReader(reader))
                {
                    cloudEventMessageEnvelope = JsonMessageSerializer.Deserializer.Deserialize<CloudEventMessageEnvelope>(jsonReader);
                }

                if (!cloudEventMessageEnvelope.Type.Equals(MessageSourceType))
                {
                    throw new SerializationException($"Message source should originate from Dapr ({MessageSourceType})");
                }

                // Upwrap the CloudEvent envelope and continue as normal with the MassTransit envelope.
                var massTransitEnvelope = cloudEventMessageEnvelope.Data;

                return new JsonConsumeContext(JsonMessageSerializer.Deserializer, receiveContext, massTransitEnvelope);
            }
            catch (JsonSerializationException ex)
            {
                throw new SerializationException("A JSON serialization exception occurred while deserializing the message", ex);
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SerializationException("An exception occurred while deserializing the message", ex);
            }
        }

        static Encoding GetMessageEncoding(ReceiveContext receiveContext)
        {
            var contentEncoding = receiveContext.TransportHeaders.Get("Content-Encoding", default(string));

            return string.IsNullOrWhiteSpace(contentEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(contentEncoding);
        }
    }
}