using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace customerAPI.Controllers
{
    /// <summary>
    /// Controller: Home
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Redirect to swagger
        /// </summary>
        /// <returns>Swagger page</returns>
        [Route("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}