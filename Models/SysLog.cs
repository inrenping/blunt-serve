using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace BluntServe.Models
{
    public class SysLog
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("user_id")]
        public string? UserId { get; set; }

        [Column("user_name")]
        public string? UserName { get; set; }

        [Required]
        [Column("log_type")]
        public string LogType { get; set; } = string.Empty;

        [Column("module_name")]
        public string? ModuleName { get; set; }

        [Column("op_desc")]
        public string? OpDesc { get; set; }

        [Column("req_url")]
        public string? ReqUrl { get; set; }

        [Column("req_method")]
        public string? ReqMethod { get; set; }

        [Column("req_params", TypeName = "jsonb")]
        public string? ReqParams { get; set; }
        [Column("resp_data", TypeName = "jsonb")]
        public string? RespData { get; set; }

        [Column("ip_address", TypeName = "inet")]
        public IPAddress? IpAddress { get; set; }

        [Column("user_agent")]
        public string? UserAgent { get; set; }

        [Column("duration_ms")]
        public int DurationMs { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
