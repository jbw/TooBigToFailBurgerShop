using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Activities
{
    public class CreateBurgerOrderActivity : IActivity<CreateBurgerOrderArguments, CreateBurgerOrderLog>
    {
        readonly ILogger<CreateBurgerOrderActivity> _logger;

        public CreateBurgerOrderActivity(ILogger<CreateBurgerOrderActivity> logger)
        {
            _logger = logger;
        }

        public Task<CompensationResult> Compensate(CompensateContext<CreateBurgerOrderLog> context)
        {
            _logger.LogInformation($"CreateBurgerOrderActivity {context.CorrelationId}");

            return Task.FromResult(context.Compensated());
        }

        public Task<ExecutionResult> Execute(ExecuteContext<CreateBurgerOrderArguments> context)
        {
            return Task.FromResult(context.Completed<CreateBurgerOrderLog>( new { }));
        }
    }
}