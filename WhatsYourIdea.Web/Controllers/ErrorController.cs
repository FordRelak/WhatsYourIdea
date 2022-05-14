using Microsoft.AspNetCore.Mvc;
using WhatsYourIdea.Web.Constants;
using WhatsYourIdea.Web.Models;

namespace WhatsYourIdea.Web.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        public ErrorController()
        {
        }

        [HttpGet("{code}")]
        public IActionResult Index(int code)
        {
            var message = ErrorMessageConstant.GetMessage(code);
            return View(new ErrorModel(code, message));
        }
    }
}