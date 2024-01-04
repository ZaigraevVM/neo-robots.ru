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
        private readonly Timer _timerNews;
        private readonly Timer _timerNewsList;
        private readonly IServiceProvider _provider;
        public CronManager(ILogger<CronManager> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
            _timerNews = new Timer(i => RunImportNews(), null, new TimeSpan(1), TimeSpan.FromHours(1));
            //_timerNewsList = new Timer(i => RunImportNewsList(), null, new TimeSpan(1), TimeSpan.FromHours(12));
            _timerNewsList = new Timer(i => RunImportNewsList(), null, new TimeSpan(1), TimeSpan.FromSeconds(12));
        }

        private void RunImportNews()
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

        private void RunImportNewsList()
        {
            try
            {
                using (var scope = _provider.CreateScope())
                {
                    Action<IServiceProvider> action = provider => provider.GetService<IAggregatorManager>().ImportNewsListAsync();
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
