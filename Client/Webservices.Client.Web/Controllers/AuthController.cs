using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Webservices.Client.Web.Models;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SigninInput input)
        {
            if(!ModelState.IsValid)
                return View();

            var response = await _identityService.SignIn(input);

            if (!response.IsSuccessful)
            {
                response.Errors.ForEach(x =>
                {
                    ModelState.AddModelError(String.Empty,x);
                });
                return View();
            }
                

            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> Logut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _identityService.RevokeRefreshToken();

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
