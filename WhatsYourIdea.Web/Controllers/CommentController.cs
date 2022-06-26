using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsYourIdea.Applications.Services;
using WhatsYourIdea.Web.ViewModels;

namespace WhatsYourIdea.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public CommentController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel model)
        {
            if(model.Hash is not null
                && model.Text is not null)
            {
                await _unitOfWorkService.CommentService.AddCommentToIdea(model.Hash, model.Text, User.Identity.Name);
            }

            return RedirectToAction("Index", "Idea", new { hash = model.Hash });
        }
    }
}