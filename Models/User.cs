using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BluntServe.Models
{
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("user_name")]
        public string UserName { get; set; } = string.Empty;
        [Column("user_email")]
        public string UserEmail { get; set; } = string.Empty;
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;
        [Column("create_time")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        [Column("update_time")]
        public DateTime? UpdateTime { get; set; }
        [Column("active")]
        public bool Active { get; set; } = true;
        [NotMapped]
        public List<string> Roles { get; set; } = new();
    }
}
