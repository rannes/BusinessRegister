using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessRegister.Dal.Models;
using BusinessRegister.Dal.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessRegister.Api.Services
{
    /// <summary>
    /// Download and update with new data from opendata.riik.ee
    /// <remarks>
    /// https://opendata.riik.ee/dataset/http-avaandmed-rik-ee-andmed-ariregister
    /// http://avaandmed.rik.ee/andmed/ARIREGISTER/
    /// </remarks>
    /// </summary>
    public class RegistryLoaderService : BackgroundService
    {
        private readonly ILogger<RegistryLoaderService> _logger;
        private readonly ConnectionString _connectionString;
        private readonly TimeSpan _syncDelay = new TimeSpan(1, 0, 0); //1 Hour
        private readonly IDatabaseSetupRepository _databaseSetupRepository;

        /// <inheritdoc />
        public RegistryLoaderService(IOptions<ConnectionString> databaseConnectionStrings, 
            ILogger<RegistryLoaderService> logger)
        {
            _logger = logger;
            _connectionString = databaseConnectionStrings?.Value;
            _databaseSetupRepository = new DatabaseSetupRepository(databaseConnectionStrings?.Value, logger);
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Registry loader is starting");

            stoppingToken.Register(() =>
                _logger.LogDebug("Registry loader caneellation token set. Stopping service."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
                await Task.Delay(_syncDelay, stoppingToken);
            }

            _logger.LogDebug("Registry loader background task is stopping.");
        }
    }
}