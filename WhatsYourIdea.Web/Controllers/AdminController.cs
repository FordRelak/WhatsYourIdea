using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsYourIdea.Applications.Auth;
using WhatsYourIdea.Applications.Services;
using WhatsYourIdea.Web.Models;

namespace WhatsYourIdea.Web.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWorkService _unitOfWorkService;
        private readonly IUserService _userService;

        public AdminController(IUnitOfWorkService unitOfWorkService, IUserService userService)
        {
            _unitOfWorkService = unitOfWorkService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsers();
            var unVerifiedIdeas = await _unitOfWorkService.IdeaService.GetUnPublicIdeas();
            var publishedIdeas = await _unitOfWorkService.IdeaService.GetIdeasAsync("new");

            var model = new AdminModel()
            {
                UnVerifiedIdeas = unVerifiedIdeas,
                Users = users,
                PublishedIdeas = publishedIdeas
            };

            return View(model);
        }

        [HttpGet("{hash}")]
        public async Task<IActionResult> CheckIdea(string hash)
        {
            var idea = await _unitOfWorkService.IdeaService.GetIdeaForCheck(hash);

            return View(idea);
        }

        [HttpGet("Public/{hash}")]
        public async Task<IActionResult> PublicIdea(string hash)
        {
            await _unitOfWorkService.IdeaService.VerifiedIdea(hash);
            return RedirectToAction("Index");
        }

        [HttpGet("Delete/{hash}")]
        public async Task<IActionResult> DeleteIdea(string hash)
        {
            await _unitOfWorkService.IdeaService.DeleteIdeaAsync(hash);
            return RedirectToAction("Index");
        }

        [HttpGet("DeleteUser/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            await _userService.DeleteUser(username);
            return RedirectToAction("Index");
        }
    }
}