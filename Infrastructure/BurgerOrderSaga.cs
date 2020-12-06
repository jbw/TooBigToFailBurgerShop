using MassTransit;
using MassTransit.Saga;
using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Application.Messages;

namespace TooBigToFailBurgerShop
{
    public class BurgerOrderSaga : ISaga, InitiatedBy<CreateOrder>
    {
        public Guid CorrelationId { get; set; }
        public DateTime? SubmitDate { get; set; }

        public async Task Consume(ConsumeContext<CreateOrder> context)
        {
            SubmitDate = context.Message.OrderDate;

        }
    }
}
