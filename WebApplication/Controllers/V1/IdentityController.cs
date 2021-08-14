using API.Contracts.V1;
using API.Models.V1.Request;
using API.Models.V1.Response;
using API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Create([FromBody]UserRegisterRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    ErrorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authResult = await _identityService.RegisterAsync(request.email, request.email);
            if (!authResult.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    ErrorMessages = authResult.ErrorMessages
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResult.Token
            });
        }

       
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login(UserRegisterRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _identityService.LoginAsync(request.email, request.password);
            if (!authResult.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    ErrorMessages = authResult.ErrorMessages
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResult.Token,
                Success= true
            });
        }
    }
}