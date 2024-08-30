using Appointment.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointment.Presentation
{
    // DocumentsController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment()
        {
            return null;
        }

        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetAppointment(string documentId)
        {
            _appointmentService.Get(documentId);
            return null;
        }

    }

}
