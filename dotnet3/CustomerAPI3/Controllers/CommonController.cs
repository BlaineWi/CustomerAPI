using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace CustomerAPI3.Controllers
{
    /// <summary>
    /// Shared Operations for All Web Services
    /// </summary>
    [Route("/")]
    [ApiController]
    [ApiExplorerSettings(GroupName ="common")]
    public class CommonController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        public CommonController(ILogger<CommonController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get Semantic Version
        /// </summary>
        /// <returns>String of Semantic Version</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult VersionGet()
        {
            _logger.LogInformation($"Semantic Version: {Program.ProgramMetadata.SemanticVersion}");
            return Ok(Program.ProgramMetadata.SemanticVersion);
        }

        /// <summary>
        /// Health Check Endpoint
        /// </summary>
        /// <returns>HealthCheckResult</returns>
        [HttpGet("health")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public HealthCheckResult CheckHealth()
        {
            var healthCheckResultIsHealthy = true;

            if(healthCheckResultIsHealthy)
            {
                _logger.LogInformation("Healthy");
                return HealthCheckResult.Healthy("All is good.");
            }
            _logger.LogWarning("Unhealthy");
            return HealthCheckResult.Unhealthy("Not Good At All");
        }

    }
}