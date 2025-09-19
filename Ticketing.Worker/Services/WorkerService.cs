using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ticketing.Worker.Configuration;
using Ticketing.Worker.Jobs;

namespace Ticketing.Worker.Services
{
    internal class WorkerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WorkerService> _logger;
        private readonly WorkerSettings _settings;

        public WorkerService(
            IServiceScopeFactory scopeFactory,
            ILogger<WorkerService> logger,
            IOptions<WorkerSettings> options)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _settings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var jobs = scope.ServiceProvider.GetServices<IWorkerJob>();

                foreach (var job in jobs)
                {
                    _logger.LogInformation("Running job: {JobName}", job.Name);

                    try
                    {
                        await job.ExecuteAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error running job: {JobName}", job.Name);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(_settings.EventExpirationJobIntervalMinutes), stoppingToken);
            }
        }
    }
}

