using BluntServe.Services;
using BluntServe.ViewModels;
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

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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


        //[HttpPost("refresh")]
        //public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        //{
        //    // 验证 refresh token（从数据库或Redis）
        //    var isValid = await ValidateRefreshToken(request.RefreshToken);
        //    if (!isValid)
        //        return Unauthorized(new { message = "Refresh token 无效或已过期" });

        //    // 从refresh token中解析用户信息
        //    var userId = ExtractUserIdFromRefreshToken(request.RefreshToken);
        //    var user = await _userService.GetUserByIdAsync(userId);

        //    // 生成新的access token
        //    var newAccessToken = _tokenService.GenerateToken(user);

        //    // 可选的：生成新的refresh token（滚动刷新）
        //    var newRefreshToken = GenerateRefreshToken(user.Id);

        //    return Ok(new
        //    {
        //        access_token = newAccessToken,
        //        token_type = "Bearer",
        //        expires_in = 3600,
        //        refresh_token = newRefreshToken
        //    });
        //}

    }

}
