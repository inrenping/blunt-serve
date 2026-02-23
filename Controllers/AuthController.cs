using BluntServe.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BluntServe.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// 一个简单的测试接口
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("hello")]
        public IActionResult Hello() => Ok("Hello World");

        /// <summary>
        /// 登录前检查账户有效性
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("check")]
        public async Task<ActionResult> ValidateEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { message = "邮箱地址不能为空" });
            }
            var user = await _authService.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new { message = "该邮箱尚未注册" });
            }

            if (!user.Active)
            {
                return BadRequest(new { message = "该账号已被禁用" });
            }

            return Ok(new
            {
                message = "邮箱有效",
                exists = true,
                username = user.UserName
            });
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.ValidateUserAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "用户名或密码错误" });
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // 保存刷新令牌
            await _authService.SaveRefreshTokenAsync(
                user.UserId,
                refreshToken,
                DateTime.UtcNow.AddDays(7)
            );
            var userResponse = new
            {
                Id = user.UserId,
                Username = user.UserName,
                Email = user.UserEmail,
                Roles = user.Roles,
                CreatedAt = user.CreatedAt
            };

            return Ok(new
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.UtcNow.AddMinutes(60),
                User = userResponse
            });
        }
        /// <summary>
        /// 刷新 Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] RefreshRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { message = "Refresh Token 不能为空" });
            }
            var storedToken = await _authService.GetRefreshTokenAsync(request.RefreshToken);
            if (storedToken == null || storedToken.ExpiresTime < DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Refresh Token 已失效，请重新登录" });
            }
            var user = await _authService.GetUserByIdAsync(storedToken.UserId.ToString());
            if (user == null || !user.Active)
            {
                return Unauthorized(new { message = "用户不存在或已被禁用" });
            }
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            await _authService.RevokedRefreshTokenAsync(request.RefreshToken);
            // 保存刷新令牌
            await _authService.SaveRefreshTokenAsync(
                user.UserId,
                newRefreshToken,
                DateTime.UtcNow.AddDays(7)
            );
            return Ok(new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        [HttpGet("me")]
        public async Task<ActionResult> GetCurrentUserInfo()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "无效的令牌信息" });
            }
            var userId = userIdClaim.Value;
            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { message = "用户不存在" });
            }
            if (!user.Active)
            {
                return Unauthorized(new { message = "用户状态异常" });
            }
            return Ok(new
            {
                Id = user.UserId,
                Username = user.UserName,
                Email = user.UserEmail,
                Roles = user.Roles,
                LastLoginTime = DateTime.Now
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }
            await _authService.RevokeAllUserTokensAsync(userIdClaim);
            return Ok(new { message = "已安全退出所有设备" });
        }
    }
}
