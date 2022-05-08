using Microsoft.AspNetCore.Mvc;
using WhatsYourIdea.Applications.Services;

namespace WhatsYourIdea.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public HomeController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string option = "new")
        {
            var userName = User.Identity.Name;
            var ideas = await _unitOfWorkService.IdeaService.GetIdeasAsync(option, userName);
            return View(ideas);
        }
    }
}