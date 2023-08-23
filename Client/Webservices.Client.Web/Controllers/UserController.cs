using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userService.GetUser());
        }
    }
}
