using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerAPI3.Controllers
{
    /// <summary>
    /// Customer Operations
    /// </summary>
    [ApiController]
    [Route("/v2/Person")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = Startup.MinorVersion)]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        public CustomerController(ILogger<CustomerController> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Id List (get)
        /// </summary>
        /// <returns></returns>
        [HttpGet("IdList")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public IEnumerable<string> IdList()
        {
            this._logger.LogInformation("IdList()");
            var model = DataAccess.DataFactory.People.Select(m => m._id).ToList();
            return model;
        }

        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="id">Id to get</param>
        /// <returns>Person or null</returns>
        /// <response code="200">Person</response>
        /// <response code="400">Bad ID value</response>
        /// <response code="404">ID Not Found</response>
        [ProducesResponseType(typeof(Models.Customer), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [HttpGet("{id}")]
        public Models.Customer Get(string id)
        {
            this._logger.LogInformation("Get: {0}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("id must be a valid value", nameof(id));
            }
            var model = DataAccess.DataFactory.People.Where(p => p._id == id).FirstOrDefault();
            if (model == null)
            {
                throw new KeyNotFoundException("No Key Found Matching: " + id);
            }
            return model;
        }

        /// <summary>
        /// Search for people
        /// </summary>
        /// <param name="text">Search Text</param>
        /// <returns>Search results</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Missing Search Text</response>
        [ProducesResponseType(typeof(IEnumerable<Models.Customer>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [HttpGet("Search/{text}")]
        public IEnumerable<Models.Customer> Search(string text)
        {
            this._logger.LogInformation("Search: {0}", text);

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Search Text must be provided", nameof(text));
            }

            var results = DataAccess.DataFactory.People.Where(p => ((p.NameFirst.Contains(text) || p.NameLast.Contains(text) || p.EMail.Contains(text)))).ToList();

            return results;
        }

        /// <summary>
        /// Return the 1st match of a search
        /// </summary>
        /// <param name="text">Search Text</param>
        /// <returns>First Customer that Matches</returns>
        [ProducesResponseType(typeof(Models.Customer), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [HttpGet("First/{text}")]
        public Models.Customer First(string text)
        {
            this._logger.LogInformation("Search: {0}", text);

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Search Text must be provided", nameof(text));
            }

            var model = DataAccess.DataFactory.People.Where(p => ((p.NameFirst.Contains(text) || p.NameLast.Contains(text) || p.EMail.Contains(text)))).FirstOrDefault();
            
            return model;

        }

    }
}