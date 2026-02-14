using BluntServe.Models;

namespace BluntServe.Services
{
    /// <summary>
    /// Token 处理
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 生成 Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateAccessToken(User user);
        /// <summary>
        /// 生成 RefreshToken
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();
    }
}
