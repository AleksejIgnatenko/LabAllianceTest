using LabAllianceTest.Models;

namespace LabAllianceTest.Abstractions
{
    public interface IUserService
    {
        Task<(string message, int statusCode)> RegistrationUserAsync(UserModel user);
        Task<(string message, int statusCode)> LoginUserAsync(UserModel user);
        Task<List<UserModel>?> GetAllUsersAsync();
        Task<(string message, int statusCode)> RefreshToken();
    }
}