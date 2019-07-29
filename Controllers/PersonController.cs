using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace customerAPI.Controllers
{
    /// <summary>
    /// Person Operations
    /// </summary>
    [Produces("application/json")]
    [Route("Person")]
    public class PersonController : Controller
    {
        private ILogger<PersonController> _logger = null;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="id">Id to get</param>
        /// <returns>Person or null</returns>
        /// <response code="200">Person</response>
        /// <response code="400">Bad ID value</response>
        /// <response code="404">ID Not Found</response>
        [ProducesResponseType(typeof(Models.Person), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [HttpGet("{id}")]
        public Models.Person Get(string id)
        {
            _logger.LogInformation("Get: {0}", id);

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("id must be a valid value", nameof(id));
            }
            var model = DataAccess.DataFactory.People.Where(p => p._id == id).FirstOrDefault();
            if(model == null)
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
        [ProducesResponseType(typeof(IEnumerable<Models.Person>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        [HttpGet("Search/{text}")]
        public IEnumerable<Models.Person> Search(string text)
        {
            _logger.LogInformation("Search: {0}", text);

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Search Text must be provided", nameof(text));
            }

            var results = DataAccess.DataFactory.People.Where(p => ((p.NameFirst.Contains(text) || p.NameLast.Contains(text) || p.EMail.Contains(text)))).ToList();
            return results;
        }

        /// <summary>
        /// Delete a person
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <response code="200">(ok)</response>
        /// <response code="204">(deleted)</response>
        /// <response code="404">ID Not Found</response>
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(void), 404)]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _logger.LogInformation("Delete: {0}", id);

            var model = DataAccess.DataFactory.People.Where(p => p._id == id).FirstOrDefault();
            if (model == null)
            {
                throw new KeyNotFoundException("No Key Found Matching: " + id);
            }
            DataAccess.DataFactory.PersonList.Remove(model);
            Response.StatusCode = (int)HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Add / Update a Person Record. 
        /// If adding use the empty string or empty GUID as <c>_id</c>
        /// </summary>
        /// <param name="model">Person</param>
        /// <returns>Person</returns>
        /// <response code="200">Person</response>
        /// <response code="201">Person</response>
        /// <response code="404">Not Found</response>
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(Models.Person), 200)]
        [ProducesResponseType(typeof(Models.Person), 201)]
        [ProducesResponseType(typeof(void), 404)]
        public Models.Person AddUpdate(Models.Person model)
        {
            _logger.LogInformation("Add/Update: {0}", model._id);

            var model2 = DataAccess.DataFactory.People.Where(p => p._id == model._id).FirstOrDefault();

            if ((string.IsNullOrWhiteSpace(model._id)) || (model._id == Guid.Empty.ToString()) || (model2 == null))
            {
                if ((string.IsNullOrWhiteSpace(model._id)) || (model._id == Guid.Empty.ToString())) model._id = Guid.NewGuid().ToString();
                DataAccess.DataFactory.PersonList.Add(model);
                Response.StatusCode = (int)HttpStatusCode.Created;
            } else
            {
                DataAccess.DataFactory.PersonList.Remove(model2);
                DataAccess.DataFactory.PersonList.Add(model);
            }

            return model;
        }

    }
}