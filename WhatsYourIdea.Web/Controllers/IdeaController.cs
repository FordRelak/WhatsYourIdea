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

        [HttpGet("{ideaHash}")]
        public async Task<IActionResult> Index(string ideaHash)
        {
            if(ideaHash is null)
            {
                return Redirect("/404");
            }

            var idea = await _unitOfWorkService.IdeaService.GetPublicIdeaByHash(ideaHash);

            return View(idea);
        }
    }
}