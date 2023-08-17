using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Validation;
using Microservices.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace Microservices.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var ifExist = await _userManager.FindByEmailAsync(context.UserName);

            if (ifExist is null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email or Password is incorrect." });

                context.Result.CustomResponse = errors;

                return;
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(ifExist, context.Password);

            if (!passwordCheck)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Password is incorrect." });

                context.Result.CustomResponse = errors;

                return;
            }

            context.Result = new GrantValidationResult(ifExist.Id.ToString(), 
                OidcConstants
                                    .AuthenticationMethods
                                    .Password);
        }
    }
}