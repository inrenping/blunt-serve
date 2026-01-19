namespace BluntServe.ViewModels
{
    public class UserVo
    {

        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; } = true;
    }
}
