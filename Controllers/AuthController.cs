using BluntServe.Services;
using BluntServe.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BluntServe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.ValidateUserAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "用户名或密码错误" });
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            //// 保存刷新令牌
            await _authService.SaveRefreshTokenAsync(
                user.Id,
                refreshToken,
                DateTime.UtcNow.AddDays(7)
            );

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Roles = user.Roles,
                CreatedAt = user.CreatedAt
            };

            return Ok(new LoginResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddMinutes(60),
                User = userResponse
            });
        }

    }

}
