
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Shared.ControllerBase
{
    public class CustomControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(ResponseDto<T> response) =>
            new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
    }
}
