using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Webservices.Client.Web.Exceptions;
using Webservices.Client.Web.Models;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService _catalogService;

        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCoursesAsycn());
        }

        public async Task<IActionResult> Detail(string courseId)
        {

            return View(await _catalogService.GetCourseByCourseIdAsync(courseId));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (errorFeature is not null && errorFeature.Error is UnauthorizeException)
                RedirectToAction(nameof(AuthController.Logut), "Auth");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}