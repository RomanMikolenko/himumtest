using System;
using System.Threading;
using System.Threading.Tasks;
using HiMum.ApiGateway.Domain.Interfaces;
using HiMum.ApiGateway.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HiMum.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> _logger;
        private readonly IEncryptionService _encryptionService;

        public GatewayController(ILogger<GatewayController> logger, IEncryptionService encryptionService)
        {
            _logger = logger;
            _encryptionService = encryptionService;
        }

        [HttpPost("encrypt")]
        public async Task<IActionResult> EncryptData([FromBody] EncryptModelViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _encryptionService.EncryptData(model.EncryptData, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during data encryption, string: {model.EncryptData}");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("decrypt")]
        public async Task<IActionResult> DecryptData([FromBody] DecryptModelViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _encryptionService.DecryptData(model.DecryptData, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during data decryption, string: {model.DecryptData}");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
