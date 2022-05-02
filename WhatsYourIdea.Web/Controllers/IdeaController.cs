using Microsoft.AspNetCore.Mvc;

namespace WhatsYourIdea.Web.Controllers
{
    [Route("[controller]")]
    public class IdeaController : Controller
    {
        public IdeaController()
        {
        }

        [HttpGet("{ideaHash}")]
        public async Task<IActionResult> Index(string ideaHash)
        {
            return View();
        }
    }
}