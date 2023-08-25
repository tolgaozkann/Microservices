using Microservices.Shared.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Webservices.Client.Web.Models.Catalog;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CourseController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _catalogService.GetAllCoursesByUserIdAsync(_sharedIdentityService.GetUserId));
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoriesAsycn();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var categories = await _catalogService.GetAllCategoriesAsycn();
            if (!ModelState.IsValid)
                return View();

            courseCreateInput.UserId =  _sharedIdentityService.GetUserId;
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");


            await _catalogService.CreateCourseAsync(courseCreateInput);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string courseId)
        {
            var course = await _catalogService.GetCourseByCourseIdAsync(courseId);
            var categories = await _catalogService.GetAllCategoriesAsycn();

            if (course is null)
                return RedirectToAction(nameof(Index));

            ViewBag.categoryList = new SelectList(categories, "Id", "Name",course.Id);
            CourseUpdateInput courseUpdateInput = new()
            {
                Name = course.Name,
                CategoryId = course.CategoryId,
                Description = course.Description,
                Price = course.Price,
                Id = course.Id,
                Picture = course.Picture,
                UserId = _sharedIdentityService.GetUserId,
                Feature = new FeatureViewModel(){Duration = course.Feature.Duration}
            };
            await _catalogService.UpdateCourseAsync(courseUpdateInput);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            var categories = await _catalogService.GetAllCategoriesAsycn();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.CategoryId);
            if (!ModelState.IsValid)
                return View();

            await _catalogService.UpdateCourseAsync(courseUpdateInput);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string courseId)
        {
            await _catalogService.DeleteCourseAsync(courseId);
            return RedirectToAction(nameof(Index));
        }
    }
}
