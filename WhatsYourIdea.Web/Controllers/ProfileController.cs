using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsYourIdea.Applications.Services;

namespace WhatsYourIdea.Web.Controllers
{
    [Authorize]
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
            var ideas = await _unitOfWorkService.IdeaService.GetIdeasByUser(User.Identity.Name);
            return View();
        }
    }
}