using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BluntServe.Models
{
    /// <summary>
    /// 用户刷新令牌实体，用于管理 JWT 的长效登录状态
    /// </summary>
    public class UserRefreshToken
    {
        /// <summary>
        /// 主键 ID
        /// </summary>
        [Key]
        [Column("id")] 
        public int Id { get; set; }

        /// <summary>
        /// 关联的用户 ID
        /// </summary>
        [Column("user_id")] 
        public int UserId { get; set; }

        /// <summary>
        /// 刷新令牌字符串
        /// </summary>
        [Column("token")] 
        public string Token { get; set; } = null!;

        /// <summary>
        /// 令牌过期时间
        /// </summary>
        [Column("expires_time")] 
        public DateTime ExpiresTime { get; set; }

        /// <summary>
        /// 令牌创建时间，默认为当前 UTC 时间
        /// </summary>
        [Column("create_time")] 
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 创建此令牌时的客户端 IP 地址
        /// </summary>
        [Column("expires_ip")]
        public string? CreatedIp { get; set; }

        /// <summary>
        /// 创建此令牌时的浏览器或设备标识符
        /// </summary>
        [Column("user_agent")] 
        public string? UserAgent { get; set; }

        /// <summary>
        /// 令牌是否已被手动撤销（如：用户点击退出登录、管理员强制下线）
        /// </summary>
        [Column("revoked")] 
        public bool Revoked { get; set; }
        /// <summary>
        /// 获取令牌是否已经超过有效期
        /// </summary>
        [NotMapped]
        public bool Expired => DateTime.UtcNow >= ExpiresTime;

        /// <summary>
        /// 获取令牌当前是否可用（既未手动撤销，也未过期）
        /// </summary>
        [NotMapped]
        public bool Active => !Revoked && !Expired;
    }
}