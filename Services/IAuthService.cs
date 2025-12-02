using BluntServe.Models;
using BluntServe.ViewModels;
using Microsoft.AspNetCore.Identity.Data;

namespace BluntServe.Services
{
    public interface IAuthService
    {

        Task<User?> ValidateUserAsync(string username, string password);
        Task<User> RegisterUserAsync(RegisterRequest request);
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken);
        Task SaveRefreshTokenAsync(int id, string refreshToken, DateTime dateTime);
    }
}
