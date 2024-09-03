﻿using DocumentUpload.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentUpload.Presentation
{
    // DocumentsController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentMetadata metadata, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            using (var stream = file.OpenReadStream())
            {
                var documentId = await _documentService.StoreDocument(metadata, stream);
                return Ok(new { DocumentId = documentId });
            }
        }

        //[HttpGet("{documentId}")]
        //public async Task<IActionResult> GetDocument(string documentId)
        //{
        //    var documentStream = await _documentService.GetDocument(documentId);
        //    if (documentStream == null)
        //        return NotFound();

        //    return File(documentStream, "application/octet-stream");
        //}

        [HttpGet("{documentId}")]
        public async Task<IActionResult> GetDocument(string documentId)
        {
            var documentStream = await _documentService.GetDocument(documentId);
            if (documentStream == null)
                return NotFound();

            // Optional: Set the content type and headers based on the file type and metadata
            var fileName = "document_" + documentId; // Customize based on your metadata
            var contentType = "application/octet-stream"; // Customize based on your metadata

            // Return a FileStreamResult to stream the file to the client
            return new FileStreamResult(documentStream, contentType)
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchDocuments([FromQuery] string query)
        {
            var results = await _documentService.SearchDocuments(query);
            return Ok(results);
        }
    }

}
