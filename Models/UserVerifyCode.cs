using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BluntServe.Models
{
    public class UserVerifyCode
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        [Column("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 用途：例如 "Register", "ResetPassword", "Login2FA"
        /// </summary>
        [Required]
        [MaxLength(20)]
        [Column("purpose")]
        public string Purpose { get; set; } = string.Empty;

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("used")]
        public bool? Used { get; set; } = false;

        /// <summary>
        /// 存储请求者的 IP 地址
        /// </summary>
        [Column("ip_address", TypeName = "inet")]
        public string? IpAddress { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
