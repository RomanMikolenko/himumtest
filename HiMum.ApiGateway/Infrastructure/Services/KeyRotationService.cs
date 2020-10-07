using HiMum.ApiGateway.Domain.Interfaces;
using HiMum.ApiGateway.Domain.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HiMum.ApiGateway.Infrastructure.Services
{
    public class KeyRotationService : IHostedService, IDisposable
    {
        private readonly ILogger<KeyRotationService> _logger;
        private readonly KeyRotationConfiguration _configuration;
        private readonly IEncryptionService _encryptionService;
        private Timer _timer;
        private int _executionCount = 0;

        public KeyRotationService(
            ILogger<KeyRotationService> logger,
            IOptions<KeyRotationConfiguration> options, 
            IEncryptionService encryptionService)
        {
            _logger = logger;
            _configuration = options.Value;
            _encryptionService = encryptionService;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Key Rotation Service running.");

            var interval = TimeSpan.FromSeconds(_configuration.RotateTimeSeconds);

            _timer = new Timer(DoWork, null, interval, interval);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Key Rotation Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);

            _logger.LogInformation("Key Rotation Service is working. Count: {Count}", count);

            _logger.LogInformation("Trying to rotate key...");

            var result = _encryptionService.RotateKey(default).GetAwaiter().GetResult();

            if (result.IsSuccess)
            {
                _logger.LogInformation("Key rotation completed successfully!");
            }
            else
            {
                _logger.LogError("Key rotation failed with errors:");
                foreach (var errors in result.Errors)
                {
                    _logger.LogError(errors.Message, errors.Details);
                }
            }
        }
    }
}
