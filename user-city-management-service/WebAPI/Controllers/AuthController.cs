using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using user_city_management_service.Application.Services;
using user_city_management_service.Application.Dtos;

namespace user_city_management_service.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="request">The login request containing username and password.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new { token });
        }
    }
}
