using BluntServe.Data;
using BluntServe.Interfaces;
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

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(u => u.UserEmail == email && u.Active);
            if (user == null) return null;
            var isPasswordValid = VerifyPassword(user, password, user.PasswordHash);
            return isPasswordValid ? user : null;
        }

        private bool VerifyPassword(User user, string password, string storedHash)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, storedHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expiresTime)
        {
            var activeTokens = await _dbContext.UserRefreshTokens
                .Where(u => u.UserId == userId && !u.Revoked && u.ExpiresTime > DateTime.UtcNow)
                .ToListAsync();
            foreach (var oldToken in activeTokens)
            {
                oldToken.Revoked = true;
            }
            var newUserRefreshToken = new UserRefreshToken
            {
                UserId = userId,
                refreshToken = refreshToken,
                ExpiresTime = expiresTime,
                CreatedAt = DateTime.UtcNow,
                Revoked = false,
                CreatedIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
                UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString()
            };
            await _dbContext.UserRefreshTokens.AddAsync(newUserRefreshToken);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            var user = await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(u => String.Equals(userId, u.UserId.ToString()) && u.Active);
            if (user == null) return null;
            return user;
        }

        public async Task<UserRefreshToken?> GetRefreshTokenAsync(string refreshToken)
        {
            var userRefreshToken = await _dbContext.UserRefreshTokens.FirstOrDefaultAsync(x => x.refreshToken == refreshToken && !x.Active);
            if (userRefreshToken == null) return null;
            return userRefreshToken;
        }

        public async Task RevokedRefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _dbContext.UserRefreshTokens
                .FirstOrDefaultAsync(x => x.refreshToken == refreshToken);

            if (storedToken != null)
            {
                storedToken.Revoked = true;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RevokeAllUserTokensAsync(string userId)
        {
            var tokens = await _dbContext.UserRefreshTokens
                .Where(t => String.Equals(userId, t.UserId.ToString()) && !t.Revoked)
                .ToListAsync();

            if (tokens.Any())
            {
                foreach (var token in tokens)
                {
                    token.Revoked = true;
                }
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
