using System.Threading.Tasks;

namespace user_city_management_service.Application.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        Task<string> AuthenticateAsync(string username, string password);
    }
}
