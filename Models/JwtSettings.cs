namespace BluntServe.Models
{
    /// <summary>
    /// JWT 认证配置项，对应 appsettings.json 中的 JWT 节点
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// 用于签名和加密 Token 的密钥（Key）
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// 令牌发行者 (Issuer)
        /// 通常是 API 的域名或标识符
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// 令牌接收者 (Audience)
        /// 通常是前端应用或客户端标识
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// 访问令牌 (Access Token) 的过期时长
        /// </summary>
        public int AccessExpiration { get; set; } = 30;

        /// <summary>
        /// 刷新令牌 (Refresh Token) 的过期时长
        /// </summary>
        public int RefreshExpiration { get; set; } = 7;
    }
}