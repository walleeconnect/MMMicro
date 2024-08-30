using Compliance.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.Presentation
{
    // DocumentsController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class ComplianceController : ControllerBase
    {
        private readonly IComplianceService _complianceService;

        public ComplianceController(IComplianceService complianceService)
        {
            _complianceService = complianceService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCompliance(ComplianceDTO compliance)
        {
            string id = await _complianceService.AddCompliance(compliance);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCompliance(string documentId)
        {
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompliances()
        {
            return null;
        }

     
    }

}
