using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BusinessRegister.Api.Services.Helpers;
using BusinessRegister.Dal.Models;
using BusinessRegister.Dal.Repositories;
using BusinessRegister.Dal.Repositories.Interfaces;
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
    public class RegistryDataUpdaterService : BackgroundService
    {
        private readonly ILogger<RegistryDataUpdaterService> _logger;
        private readonly TimeSpan _syncDelay = new TimeSpan(1, 0, 0); //1 Hour
        private readonly IDatabaseSetupRepository _databaseSetupRepository;
        private const string FileName = "ariregister_xml.zip";
        private const string FileDownloadLocation = "http://avaandmed.rik.ee/andmed/ARIREGISTER/ariregister_xml.zip";

        /// <inheritdoc />
        public RegistryDataUpdaterService(IOptions<ConnectionString> databaseConnectionStrings, 
            ILogger<RegistryDataUpdaterService> logger)
        {
            _logger = logger;
            _databaseSetupRepository = new DatabaseSetupRepository(databaseConnectionStrings?.Value, logger);
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Registry loader is starting");

            stoppingToken.Register(() =>
                _logger.LogDebug("Registry loader caneellation token set. Stopping service."));

            await SetupDatabase(stoppingToken);
            var lastModifiedTime = new DateTime();
            var zipFileLocation = string.Empty;
            var extractedFileLocation = string.Empty;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (FileHasBeenUpdated(lastModifiedTime, out lastModifiedTime))
                    {
                        zipFileLocation = FileHelper.DownloadFile(FileDownloadLocation, FileName);
                        extractedFileLocation = FileHelper.ExtractFile(zipFileLocation);
                        var companiesData = XmlHelper.DeserializeXmlFromFile(extractedFileLocation);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
                finally
                {
                    if (!string.IsNullOrWhiteSpace(zipFileLocation) && File.Exists(zipFileLocation))
                        File.Delete(zipFileLocation);
                    if (!string.IsNullOrWhiteSpace(extractedFileLocation) && File.Exists(extractedFileLocation))
                        File.Delete(extractedFileLocation);
                }

                await Task.Delay(_syncDelay, stoppingToken);
            }

            _logger.LogDebug("Registry loader background task is stopping.");
        }

        /// <summary>
        /// Try to setup the database if it fails 5 times then close the program.
        /// </summary>
        /// <returns></returns>
        private async Task SetupDatabase(CancellationToken stoppingToken)
        {
            var retryCount = 0;
            var databaseSetUp = false;

            while (!databaseSetUp)
            {
                try
                {
                    retryCount++;
                    await _databaseSetupRepository.TestDatabaseConnection();
                    await _databaseSetupRepository.DatabaseMigration();
                    databaseSetUp = true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                    if (retryCount >= 5)
                        Environment.Exit(-1);
                    await Task.Delay(new TimeSpan(0, 0, 1), stoppingToken);
                }
            }
        }

        /// <summary>
        /// Has file been updated on remote location or not.
        /// </summary>
        /// <param name="lastModifiedDateTime">When we last time updated our logical file.</param>
        /// <param name="newModifiedDateTime">Output the file new modified time.</param>
        /// <returns>If remote file is newer than our last update file.</returns>
        private bool FileHasBeenUpdated(DateTime lastModifiedDateTime, out DateTime newModifiedDateTime)
        {
            var fileModifiedTimeRaw = FileHelper.XmlRawLastModifiedDate(FileName);
            newModifiedDateTime = lastModifiedDateTime;

            if (!DateTime.TryParse(fileModifiedTimeRaw, out var fileModifiedTime))
                return false;

            newModifiedDateTime = fileModifiedTime;
            return fileModifiedTime > lastModifiedDateTime;
        }
    }
}