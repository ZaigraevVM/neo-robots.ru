using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace SMI.Managers
{
    public interface ICronManager
    {
        
    }

    public class CronManager : ICronManager
    {
        private readonly ILogger<CronManager> _logger;
        private readonly Timer _timer;
        private readonly IServiceProvider _provider;
        public CronManager(ILogger<CronManager> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
            _timer = new Timer(i => this.Run(), null, new TimeSpan(1), TimeSpan.FromHours(1));
        }

        private void Run()
        {
            try
            {
                using (var scope = _provider.CreateScope())
                {
                    Action<IServiceProvider> action = provider => provider.GetService<IAggregatorManager>().ImportNewsAsync();
                    action(scope.ServiceProvider);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error cron", ex);
            }
        }
    }
}
