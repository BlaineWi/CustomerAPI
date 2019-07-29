using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace customerAPI
{
    /// <summary>
    /// Health Check: Customer
    /// </summary>
    public class CustomerHealthCheck : IHealthCheck
    {

        private ILogger<CustomerHealthCheck> _logger = null;

        /// <summary>
        /// CTOR, add injected stuff if needed
        /// </summary>
        public CustomerHealthCheck(ILogger<CustomerHealthCheck> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Actual implementation
        /// </summary>
        /// <param name="context">HealthCheckContext</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>HealthCheckResult</returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            int ct = DataAccess.DataFactory.PersonList.Count;
            if (ct > 0)
            {
                var msg = "Person Records: " + ct.ToString();
                _logger.LogTrace("HealthCheck, {0}, {1}", msg, true);
                return Task.FromResult(HealthCheckResult.Healthy(msg));
            }
            else
            {
                var msg = "No person records.";
                _logger.LogWarning("HealthCheck, {0}, {1}", msg, false);
                return Task.FromResult(HealthCheckResult.Unhealthy());
            }
        }
    }
}
