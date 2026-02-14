using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BluntServe.Models
{
    public class UserSocial
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("provider")]
        public string Provider { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("provider_user_id")]
        public string ProviderUserId { get; set; } = string.Empty;

        [Column("access_token")]
        public string? AccessToken { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
