using BluntServe.Models;
using BluntServe.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Cryptography;

namespace BluntServe.Services
{
    public class AuthService : IAuthService
    {
        // 内存存储 - 生产环境应该使用数据库
        private readonly List<User> _users = new();
        private readonly Dictionary<int, (string refreshToken, DateTime Expiry)> _refreshTokens = new();
        private int _nextUserId = 1;

        public AuthService()
        {
            // 初始化示例用户
            InitializeSampleUsers();
        }

        /// <summary>
        /// 初始化用户列表，添加一个管理员用户。
        /// </summary>
        private void InitializeSampleUsers()
        {
            var adminPassword = HashPassword("admin123");

            _users.Add(new User
            {
                Id = _nextUserId++,
                Username = "inrenping",
                Email = "inrenping@gmail.com",
                PasswordHash = adminPassword,
                Roles = new List<string> { "Admin", "User" }
            });

        }

        /// <summary>
        /// 验证用户凭据
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var user = _users.FirstOrDefault(u =>
                u.Email == username && u.IsActive);

            if (user == null) return null;

            var isPasswordValid = VerifyPassword(password, user.PasswordHash);
            return isPasswordValid ? user : null;
        }

        /// <summary>
        /// 注册新用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<User> RegisterUserAsync(RegisterRequest request)
        {
            if (_users.Any(u => u.Email == request.Email))
                throw new ArgumentException("邮箱已存在");

            var user = new User
            {
                Id = _nextUserId++,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Roles = new List<string> { "User" }
            };

            _users.Add(user);
            return user;
        }


        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return _users.FirstOrDefault(u => u.Id == userId && u.IsActive);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.IsActive);
        }

        public async Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {

            return false;
        }

        /// <summary>
        /// 密码 Hash
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string HashPassword(string password)
        {
            //var salt = new byte[128 / 8];
            //using var rng = RandomNumberGenerator.Create();
            //rng.GetBytes(salt);

            //var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            //    password: password,
            //    salt: salt,
            //    prf: KeyDerivationPrf.HMACSHA256,
            //    iterationCount: 10000,
            //    numBytesRequested: 256 / 8));

            //return $"{Convert.ToBase64String(salt)}.{hashed}";
            return password;
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        private bool VerifyPassword(string password, string storedHash)
        {
            return true;
        }

        /// <summary>
        /// 保存 refresh token
        /// </summary>
        /// <param name="id"></param>
        /// <param name="refreshToken"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SaveRefreshTokenAsync(int id, string refreshToken, DateTime dateTime)
        {
            // TODO 改成保存到数据库
            _refreshTokens[id] = (refreshToken, dateTime);
        }
    }
}
