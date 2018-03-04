using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestfulApi.Representations;

namespace RestfulApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        [HttpGet]
        public IActionResult GetAuthors()
        {
            return new OkObjectResult(new Author[] { new Author {
                Name = "George R.R. Martin"
            }, new Author {
                Name = "Neal Stephenson"
            } }
            );
        }
    }
}
