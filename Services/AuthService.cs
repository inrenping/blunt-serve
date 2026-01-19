using BluntServe.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BluntServe.Services
{
    public class AuthService : IAuthService
    {
        private readonly PgDbContext _dbContext; 
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        public AuthService(PgDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(u => u.UserEmail == email && u.Active);
            if (user == null) return null;
            var isPasswordValid = VerifyPassword(user,password, user.PasswordHash);
            return isPasswordValid ? user : null;
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        private bool VerifyPassword(User user,string password, string storedHash)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, storedHash, password);
            return result == PasswordVerificationResult.Success;
        }
        /// <summary>
        /// 保存 刷新 Token
        /// </summary>
        /// <param name="id"></param>
        /// <param name="refreshToken"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expiresTime)
        {
            var activeTokens = await _dbContext.UserRefreshToken
                .Where(u => u.UserId == userId && !u.Revoked && u.ExpiresTime > DateTime.UtcNow)
                .ToListAsync();
            foreach (var oldToken in activeTokens)
            {
                oldToken.Revoked = true;
            }
            var newUserRefreshToken = new UserRefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                ExpiresTime = expiresTime,
                CreatedTime = DateTime.UtcNow,
                Revoked = false,
                CreatedIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
                UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString()
            };
            await _dbContext.UserRefreshToken.AddAsync(newUserRefreshToken);
            await _dbContext.SaveChangesAsync();

        }
    }
}
