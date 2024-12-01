using Microsoft.AspNetCore.Mvc;

namespace CRSAPIMAuthenticationDemo.Controllers
{
    [Route("api/crs")]
    [ApiController]
    public class CrsController : ControllerBase
    {
        public static List<string> Users = new List<string>();

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

        [HttpPost("saveUser")]
        public IActionResult SaveUser([FromBody] string user)
        {
            if (String.IsNullOrWhiteSpace(user))
                return BadRequest("Please provide Non-null or empty User value");

            if (user.Length > 30)
                return BadRequest("User value cannot be more than 30 characters");

            if (Users.Any(u => u == user.Trim()))
                return BadRequest("User already present");

            Users.Add(user);

            return Ok("User saved");
        }

        [HttpGet("getUsers")]
        public IActionResult GetUsers()
        {
            return Ok(Users);
        }
    }
}
