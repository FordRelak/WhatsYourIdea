using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsYourIdea.Applications.Services;
using WhatsYourIdea.Common;
using WhatsYourIdea.Web.Models;

namespace WhatsYourIdea.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public ProfileController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var trackedIdeas = await _unitOfWorkService.IdeaService.GetTrackedIdeas(User.Identity.Name);
            var publishedIdeas = await _unitOfWorkService.IdeaService.GetPublishedIdeas(User.Identity.Name);
            var privateIdeas = await _unitOfWorkService.IdeaService.GetPrivateIdeas(User.Identity.Name);

            var model = new ProfileModel
            {
                PrivateIdeas = privateIdeas,
                TrackedIdeas = trackedIdeas,
                PublishedIdeas = publishedIdeas,
            };

            ViewData["controller"] = nameof(ProfileController).CutController();

            return View(model);
        }

        [HttpGet("{ideaHash}")]
        public async Task<IActionResult> GetIdeaByHash(string ideaHash)
        {
            return View("Index");
        }
    }
}