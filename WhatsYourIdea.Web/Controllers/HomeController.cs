using Microsoft.AspNetCore.Mvc;
using WhatsYourIdea.Applications.Services;
using WhatsYourIdea.Web.ViewModels;

namespace WhatsYourIdea.Web.Controllers
{
    [Route("/")]
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

        [HttpGet("tag/{tagname}")]
        public async Task<IActionResult> GetIdeasByTag(string tagname)
        {
            if(tagname is null)
            {
                RedirectToAction("Index");
            }

            var userName = User.Identity.Name;
            var ideas = await _unitOfWorkService.IdeaService.GetIdeasByTag(tagname, userName);
            return View("Index", ideas);
        }

        [HttpPost("search")]
        public async Task<IActionResult> ShowIdeaBy(string search)
        {
            if(search is null)
            {
                RedirectToAction("Index");
            }

            var ideas = await _unitOfWorkService.IdeaService.SearchIdea(search, User.Identity.Name);
            return View("Index", ideas);
        }
    }
}