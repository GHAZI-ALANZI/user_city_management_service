
using user_city_management_service.Domain.Enums;

namespace user_city_management_service.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }
    public bool IsBanned { get; set; } = false;
}
