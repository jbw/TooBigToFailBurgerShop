using MassTransit.Serialization;
using System;

namespace Ordering.StateService.Application.Extensions.Dapr
{
    public class CloudEventMessageEnvelope
    {
        public Guid Id { get; init; }
        public string SpecVersion { get; init; }
        public string Type { get; init; }
        public string PubSubName { get; init; }
        public string TraceId { get; init; }
        public string DataContentType { get; init; }
        public string Source { get; init; }
        public string Topic { get; init; }
        public MessageEnvelope Data { get; init; }
    }
}