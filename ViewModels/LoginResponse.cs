namespace BluntServe.ViewModels
{
    public class LoginResponse
    {

        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public UserResponse User { get; set; } = new();
    }
}
