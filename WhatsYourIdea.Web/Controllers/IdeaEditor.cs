using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsYourIdea.Applications.Services.Interfaces;

namespace WhatsYourIdea.Web.Controllers
{
    [Route("editor")]
    [Authorize]
    public class IdeaEditor : Controller
    {
        private readonly IFileStorageService _fileStorageService;

        public IdeaEditor(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("file")]
        public async Task<IActionResult> AddFile(IFormFile file, CancellationToken cancellationToken)
        {
            var newFilePath = await _fileStorageService.CreateIdeaImageFileAsync(
                file.OpenReadStream(),
                Path.GetExtension(file.FileName),
                cancellationToken);
            return Ok(new
            {
                Location = newFilePath
            });
        }
    }
}