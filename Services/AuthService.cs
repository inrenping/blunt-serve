using BluntServe.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BluntServe.Services
{
    public class AuthService : IAuthService
    {
        private readonly PgDbContext _dbContext;

        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        public AuthService(PgDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.UserEmail == email && u.Active);
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
            // String pwd = _passwordHasher.HashPassword(user, password);
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
        public async Task SaveRefreshTokenAsync(int id, string refreshToken, DateTime dateTime)
        {
            // TODO 改成保存到数据库
            //_refreshTokens[id] = (refreshToken, dateTime);
        }
    }
}
