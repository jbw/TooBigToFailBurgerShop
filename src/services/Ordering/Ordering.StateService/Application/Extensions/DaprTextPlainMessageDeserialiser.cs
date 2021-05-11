using GreenPipes;
using MassTransit;
using MassTransit.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;

namespace Ordering.StateService
{
    public class DaprTextPlainMessageDeserialiser : IMessageDeserializer
    {
        public ContentType ContentType => new ContentType("text/plain");
        private const string MessageSourceType = "com.dapr.event.sent";

        static Encoding GetMessageEncoding(ReceiveContext receiveContext)
        {
            var contentEncoding = receiveContext.TransportHeaders.Get("Content-Encoding", default(string));

            return string.IsNullOrWhiteSpace(contentEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(contentEncoding);
        }

        public ConsumeContext Deserialize(ReceiveContext receiveContext)
        {
            try
            {
                var messageEncoding = GetMessageEncoding(receiveContext);

                using var body = receiveContext.GetBodyStream();
                using var reader = new StreamReader(body, messageEncoding, false, 1024, true);
                using var jsonReader = new JsonTextReader(reader);

                var messageToken = RawJsonMessageSerializer.Deserializer.Deserialize<JToken>(jsonReader);

                if(!messageToken.SelectToken("type").ToString().Equals("com.dapr.event.sent"))
                {
                    throw new SerializationException($"Message source should originate from Dapr ({MessageSourceType})");
                }

                return new RawJsonConsumeContext(RawJsonMessageSerializer.Deserializer, receiveContext, messageToken);
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

        public void Probe(ProbeContext context)
        {
            var scope = context.CreateScope("text");
            scope.Add("contentType", ContentType);
        }
    }
}