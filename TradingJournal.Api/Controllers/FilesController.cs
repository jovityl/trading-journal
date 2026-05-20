using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TradingJournal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/files")]
    public class FilesController : ControllerBase
    {
        private readonly string _uploadPath;

        public FilesController(IWebHostEnvironment env)
        {
            _uploadPath = Path.Combine(env.ContentRootPath, "uploads");
        }

        [HttpGet("{filename}")]
        public IActionResult GetFile(string filename)
        {
            // Prevent path traversal attacks
            var safeName = Path.GetFileName(filename);
            var fullPath = Path.Combine(_uploadPath, safeName);

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var contentType = Path.GetExtension(safeName).ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };

            return PhysicalFile(fullPath, contentType);
        }
    }
}
