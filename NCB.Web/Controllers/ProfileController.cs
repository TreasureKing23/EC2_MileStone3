using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NCB.ModelDTO;
using NCB.Repositories.Interfaces;
using NCB.Services.Interfaces;

namespace NCB.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAuthManager _authManager;
        private readonly IMapper _mapper;

        public ProfileController(IAuthManager authManager, IMapper mapper)
        {
            _authManager = authManager;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]

        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authManager.LoginAsync(model); if (result.StatusCode == 0)
            {
                ViewBag.Message = result.StatusMessage;
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]

        public async Task<IActionResult> Logout(string? returnURL)
        {
            await _authManager.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]




        [HttpGet]

        public IActionResult Register()

        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Register(UserDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authManager.RegisterAsync(model);
            if (result.StatusCode == 0)
            {
                TempData["msg"] = result.StatusMessage;
                return View(model);
            }

            return RedirectToAction("Login", "Profile");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return RedirectToAction("AccessDenied", "Administrator");
        }



    }

}
