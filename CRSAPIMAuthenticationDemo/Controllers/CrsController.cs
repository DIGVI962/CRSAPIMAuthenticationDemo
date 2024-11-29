using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRSAPIMAuthenticationDemo.Controllers
{
    [Authorize]
    [Route("api/crs")]
    [ApiController]
    public class CrsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("public")]
        public IActionResult GetPublic()
        {
            return Ok("This is a public endpoint.");
        }

        [HttpGet("secure")]
        public IActionResult GetSecure()
        {
            return Ok("This is a secure endpoint, you are authenticated!");
        }
    }
}
