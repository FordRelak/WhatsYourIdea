using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
