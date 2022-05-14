using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Applications.Services;
using WhatsYourIdea.Common;
using WhatsYourIdea.Web.Models;
using WhatsYourIdea.Web.ViewModels;

namespace WhatsYourIdea.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly IUnitOfWorkService _unitOfWorkService;
        private readonly IMapper _mapper;

        public ProfileController(IUnitOfWorkService unitOfWorkService,
                                 IMapper mapper)
        {
            _unitOfWorkService = unitOfWorkService;
            _mapper = mapper;
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
            ViewData["action"] = nameof(Detail);

            return View(model);
        }

        [HttpGet("{hash}")]
        public async Task<IActionResult> Detail(string hash)
        {
            var idea = await _unitOfWorkService.IdeaService.GetIdeaForEdit(hash, User.Identity.Name);
            return View("Detail", idea);
        }

        private async Task<IEnumerable<SelectListItem>> GenerateSelectItem(IdeaDetailedDto idea)
        {
            var tags = (await _unitOfWorkService.TagService.GetTagsExceptIdsAsync(idea.Tags.Select(x => x.Id).ToArray())).ToList();
            tags.AddRange(idea.Tags);
            return tags.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        [HttpGet("Delete/{hash}")]
        public async Task<IActionResult> DeleteIdea(string hash)
        {
            await _unitOfWorkService.IdeaService.DeleteIdeaAsync(hash, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{hash}")]
        public async Task<IActionResult> EditIdea(string hash)
        {
            var idea = await _unitOfWorkService.IdeaService.GetIdeaForEdit(hash, User.Identity.Name);
            var editorVM = _mapper.Map<EditorViewModel>(idea);
            editorVM.Tags = await GenerateSelectItem(idea);
            return View("Editor", editorVM);
        }

        [HttpGet("Public/{hash}")]
        public async Task<IActionResult> PublicIdea(string hash)
        {
            await _unitOfWorkService.IdeaService.PublicIdeaAsync(hash, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}