using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using user_city_management_service.Domain.Entities;
using user_city_management_service.Infrastructure.Data;

namespace user_city_management_service.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) => _context = context;

    public async Task<User> GetByUsernameAsync(string username) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User> GetByIdAsync(int id) =>
        await _context.Users.FindAsync(id);


    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .Select(user => new User
            {
                Username = user.Username,
                Role = user.Role,
                IsBanned = user.IsBanned
            })
            .ToListAsync();
    }



    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
