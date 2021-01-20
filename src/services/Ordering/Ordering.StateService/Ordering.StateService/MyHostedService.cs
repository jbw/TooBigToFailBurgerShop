using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.StateService
{
    public class MyHostedService : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}