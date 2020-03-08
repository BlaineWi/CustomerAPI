using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;

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
            this._logger = logger;
        }

        /// <summary>
        /// Get Semantic Version
        /// </summary>
        /// <returns>String of Semantic Version</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult VersionGet()
        {
            this._logger.LogInformation($"Semantic Version: {Program.ProgramMetadata.SemanticVersion}");
            return this.Ok(Program.ProgramMetadata.SemanticVersion);
        }

        /// <summary>
        /// Return Program Metadata and Version Info
        /// </summary>
        /// <returns>Program Metadata and Version Info</returns>
        [HttpGet("version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public IActionResult VersionInfo()
        {
            return this.Ok(Program.ProgramMetadata);
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

            var info = new Dictionary<string, object>
            {
                { "Company", Program.ProgramMetadata.Company },
                { "Copyright" , Program.ProgramMetadata.Copyright },
                { "Description", Program.ProgramMetadata.Description },
                { "MajorVersion", Program.ProgramMetadata.MajorVersion },
                { "Product", Program.ProgramMetadata.Product },
                { "SemanticVersion", Program.ProgramMetadata.SemanticVersion },
                { "FileVersion", Program.ProgramMetadata.FileVersion },
            };

            var ex = new HttpRequestException("Timeout");

            var deps = new Dictionary<string, object>
            {
                { "Dependancy1", "Ok" },
                { "Dependancy2", ex.Message },
                { "Dependancy3", "Ok" },
            };

            var data = new Dictionary<string, object>
            {
                { "Information", info },
                { "Dependancies", deps }
            };

            var hcr = new HealthCheckResult((ex != null) ? HealthStatus.Unhealthy : HealthStatus.Healthy, Program.ProgramMetadata.Product, ex, data);

            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(hcr);

            if (ex != null) this._logger.LogWarning(ex, msg);
            else this._logger.LogInformation(msg);

            return hcr;
        }

    }
}