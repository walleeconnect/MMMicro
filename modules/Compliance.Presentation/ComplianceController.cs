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
        private readonly IComplianceService _documentService;

        public ComplianceController(IComplianceService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> CreateCompliance()
        {
            return null;
        }

        [HttpGet("{documentId}")]
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
