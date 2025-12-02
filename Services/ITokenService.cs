using BluntServe.Models;

namespace BluntServe.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        (string username, int userId)? ValidateToken(string token);
    }
}
