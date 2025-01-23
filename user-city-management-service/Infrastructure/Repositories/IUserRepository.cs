using System.Threading.Tasks;
using user_city_management_service.Domain.Entities;

namespace user_city_management_service.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByIdAsync(int id);
    Task<List<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task DeleteAsync(User user);
    Task SaveChangesAsync();
}
