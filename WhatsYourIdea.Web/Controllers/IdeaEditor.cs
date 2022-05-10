using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Applications.Services;
using WhatsYourIdea.Applications.Services.Interfaces;
using WhatsYourIdea.Common;
using WhatsYourIdea.Web.ViewModels;

namespace WhatsYourIdea.Web.Controllers
{
    [Route("editor")]
    [Authorize]
    public class IdeaEditor : Controller
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkService _unitOfWorkService;

        public IdeaEditor(IFileStorageService fileStorageService, IMapper mapper, IUnitOfWorkService unitOfWorkService)
        {
            _fileStorageService = fileStorageService;
            _mapper = mapper;
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var tags = _unitOfWorkService.TagService.GetTags();
            
            var vm = new EditorViewModel
            {
                Tags = tags.Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            };

            return View(vm);
        }

        [HttpPost("file")]
        public async Task<IActionResult> AddFile(IFormFile file, CancellationToken cancellationToken)
        {
            var newFilePath = await _fileStorageService.CreateIdeaImageFileAsync(
                file.OpenReadStream(),
                Path.GetExtension(file.FileName),
                cancellationToken);
            return Ok(new
            {
                Location = newFilePath
            });
        }

        [HttpPost(nameof(Save))]
        public async Task<IActionResult> Save(EditorViewModel model, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var dto = _mapper.Map<CreateIdeaDto>(model);
            dto.UserName = User.Identity.Name;

            if(model.MainImage is not null)
            {
                dto.MainImagePath = await _fileStorageService.CreateIdeaImageFileAsync(
                    model.MainImage.OpenReadStream(),
                    Path.GetExtension(model.MainImage.FileName),
                    cancellationToken);
            }

            await _unitOfWorkService.IdeaService.SaveIdea(dto, cancellationToken);

            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
        }

        [HttpGet("{hashId}")]
        public async Task<IActionResult> GetByHash(string hashId)
        {
            return Ok();
        }
    }
}