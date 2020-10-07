using System;
using System.Threading.Tasks;
using HiMum.EncryptionService.Domain.Interfaces;
using HiMum.EncryptionService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HiMum.EncryptionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ILogger<SecurityController> _logger;
        private readonly IHashingService _hashingService;

        public SecurityController(ILogger<SecurityController> logger, IHashingService hashingService)
        {
            _logger = logger;
            _hashingService = hashingService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost("encrypt")]
        public async Task<IActionResult> EncryptData([FromBody] EncryptModelViewModel model)
        {
            var result = await _hashingService.EncryptData(model.EncryptData);
            return Ok(result);
        }

        [HttpPost("decrypt")]
        public async Task<IActionResult> DecryptData([FromBody] DecryptModelViewModel model)
        {
            try
            {
                var result = await _hashingService.DecryptData(model.DecryptData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during data decryption, string: {model.DecryptData}");

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("rotate-key")]
        public IActionResult RotateKey()
        {
            _hashingService.GenerateSalt();
            return Ok();
        }
    }
}
