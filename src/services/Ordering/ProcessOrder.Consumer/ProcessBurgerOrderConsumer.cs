using MassTransit;
using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Messages;

namespace TooBigToFailBurgerShop.ProcessOrder.Consumer
{
    public class ProcessBurgerOrderConsumer : IConsumer<ProcessBurgerOrder>
    {
        public Task Consume(ConsumeContext<ProcessBurgerOrder> context)
        {
            throw new NotImplementedException();
        }
    }
}
