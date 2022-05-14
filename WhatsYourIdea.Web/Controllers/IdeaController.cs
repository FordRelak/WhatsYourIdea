using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsYourIdea.Applications.Services;

namespace WhatsYourIdea.Web.Controllers
{
    [Route("[controller]")]
    public class IdeaController : Controller
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public IdeaController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet("{hash}")]
        public async Task<IActionResult> Index(string hash)
        {
            var idea = await _unitOfWorkService.IdeaService.GetPublicIdeaByHash(hash, User.Identity.Name);
            return View(idea);
        }

        [Authorize]
        [HttpGet("track/{hash}")]
        public async Task<IActionResult> TrackIdea(string hash, string returnUrl)
        {
            await _unitOfWorkService.IdeaService.TrackIdeaAsync(hash, User.Identity.Name);
            return Redirect(returnUrl);
        }

        [Authorize]
        [HttpGet("untrack/{hash}")]
        public async Task<IActionResult> UnTrackIdea(string hash, string returnUrl)
        {
            await _unitOfWorkService.IdeaService.UnTrackIdeaAsync(hash, User.Identity.Name);
            return Redirect(returnUrl);
        }
    }
}