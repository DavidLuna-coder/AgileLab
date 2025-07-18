using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TFG.Application.Services.Experiences.Commands
{
    /// <summary>
    /// Servicio en background que ejecuta periódicamente el registro de snapshots.
    /// </summary>
    public class ProjectSnapshotBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProjectSnapshotBackgroundService> _logger;
        private readonly TimeSpan _interval;

        public ProjectSnapshotBackgroundService(IServiceProvider serviceProvider, ILogger<ProjectSnapshotBackgroundService> logger, TimeSpan? interval = null)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _interval = interval ?? TimeSpan.FromHours(1); // Por defecto cada hora
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ProjectSnapshotBackgroundService iniciado");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var snapshotService = scope.ServiceProvider.GetRequiredService<ProjectSnapshotService>();
                        await snapshotService.RegisterSnapshotsAsync(stoppingToken);
                    }
                    _logger.LogInformation("Snapshots registrados correctamente a las {Time}", DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error registrando snapshots");
                }
                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
