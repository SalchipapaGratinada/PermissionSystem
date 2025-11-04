using Microsoft.AspNetCore.Mvc;
using PermissionSystem.Application.Services.Interfaces;
using PermissionSystem.Contracts.DTOs.Auth;

namespace PermissionSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// The authentication service used for user login.
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService"></param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Logs in a user with the provided credentials.
        /// </summary>
        /// <param name="dto">The login data transfer object.</param>
        /// <returns>The authentication result containing the JWT token.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto.Username, dto.Password);
            if (result == null) return Unauthorized("Usuario o contraseña incorrecta");
            return Ok(result);
        }
    }
}
