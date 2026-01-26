using BluntServe.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace BluntServe.Services
{
    /// <summary>
    /// 身份认证服务接口，负责用户登录验证、注册及令牌管理
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<User?> ValidateUserAsync(string email, string password);

        /// <summary>
        /// 保存 RefreshToken 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="refreshToken"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        Task SaveRefreshTokenAsync(int id, string refreshToken, DateTime dateTime);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User?> GetUserByIdAsync(string userId);
    }
}
