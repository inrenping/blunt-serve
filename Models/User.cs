using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BluntServe.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User
    {
        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Column("user_name")]
        public string UserName { get; set; } = string.Empty;
        [Column("user_email")]
        public string UserEmail { get; set; } = string.Empty;
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Column("active")]
        public bool Active { get; set; } = true;
        [NotMapped]
        public List<string> Roles { get; set; } = new();
    }
}
