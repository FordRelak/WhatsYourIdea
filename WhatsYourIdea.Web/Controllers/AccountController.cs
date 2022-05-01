using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WhatsYourIdea.Applications.Auth;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Web.ViewModels;

namespace WhatsYourIdea.Web.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;

        public AccountController(AuthService authService, IUserService userService, IMapper mapper)
        {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var userDto = _mapper.Map<UserDto>(model);
            var result = await _userService.CreateAsync(userDto);
            if (result.IsSuccess)
            {
                await _authService.SignInAsync(userDto);
                return RedirectToAction("Index", "Home");
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            return View(new LoginViewModel());
        }
    }
}