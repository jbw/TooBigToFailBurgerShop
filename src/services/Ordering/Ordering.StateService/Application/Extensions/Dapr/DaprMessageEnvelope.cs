using MassTransit.Serialization;


namespace Ordering.StateService.Application.Extensions.Dapr
{

    public class DaprMessageEnvelope
    {
        public string Type { get; set; }
        public MessageEnvelope Data { get; set; }
    }

}