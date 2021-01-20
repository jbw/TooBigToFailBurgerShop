using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Activities
{
    public class ProcessBurgerOrderActivity : IActivity<ProcessBurgerOrderArguments, ProcessBurgerOrderLog>
    {
        readonly ILogger<ProcessBurgerOrderActivity> _logger;

        public ProcessBurgerOrderActivity(ILogger<ProcessBurgerOrderActivity> logger)
        {
            _logger = logger;
        }

        public Task<CompensationResult> Compensate(CompensateContext<ProcessBurgerOrderLog> context)
        {
            _logger.LogInformation($"ProcessBurgerOrderActivity compensated {context.Message.TrackingNumber}");

            return Task.FromResult(context.Compensated());
        }

        public Task<ExecutionResult> Execute(ExecuteContext<ProcessBurgerOrderArguments> context)
        {
            _logger.LogInformation($"ProcessBurgerOrderActivity executed {context.Message.TrackingNumber}");

            return Task.FromResult(context.Completed<ProcessBurgerOrderLog>( new { }));
        }
    }
}