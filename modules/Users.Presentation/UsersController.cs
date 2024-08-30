using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Users.Presentation
{
    // DocumentsController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser()
        {
            return null;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return null;
        }

    }

}
