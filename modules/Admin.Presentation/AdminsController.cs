using Admin.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Presentation
{
    // DocumentsController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminService _appointmentService;

        public AdminsController(IAdminService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> GetGroups()
        {
            return null;
        }

    }

}
