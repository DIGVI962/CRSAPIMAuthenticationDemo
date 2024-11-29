using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRSAPIMAuthenticationDemo.Controllers
{
    [Route("api/crs")]
    [ApiController]
    public class CrsController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult GetPublic()
        {
            return Ok("This is a public endpoint.");
        }

        [HttpGet("secure")]
        [Authorize]
        public IActionResult GetSecure()
        {
            return Ok("This is a secure endpoint, you are authenticated!");
        }
    }
}
