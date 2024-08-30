using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Notify.Presentation
{
    // DocumentsController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class NotifyController : ControllerBase
    {

        [HttpPost("create")]
        public async Task<IActionResult> SendEmail()
        {
            return null;
        }


    }

}
