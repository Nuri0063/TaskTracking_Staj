using TaskTracking.Staj.Models;

namespace TaskTracking.Staj.Interfaces
{
    public interface IAuthService
    {
        Task<bool> UserExists(string username);
        Task<User> Register(User user, string password);
        Task<string> Login(string username, string password);
    }
}
