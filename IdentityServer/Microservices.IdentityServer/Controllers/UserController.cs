using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microservices.IdentityServer.Dtos;
using Microservices.IdentityServer.Models;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.IdentityServer.Controllers
{
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDto signUpDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                City = signUpDto.City,
                UserName = signUpDto.UserName,
                Email = signUpDto.Email,
            };

            var result = await _userManager.CreateAsync(user, signUpDto.Password);



            if (!result.Succeeded)
                return BadRequest(ResponseDto<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(),
                    400));

            return StatusCode(201);
        }
    }
}
