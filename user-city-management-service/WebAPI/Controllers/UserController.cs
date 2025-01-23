using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using user_city_management_service.Infrastructure.Repositories;
using user_city_management_service.Domain.Entities;
using user_city_management_service.Application.Dtos;
using user_city_management_service.Domain.Enums;

namespace user_city_management_service.WebAPI.Controllers;

[Authorize] //  Requires Authentication for all actions
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    //  Check if Authenticated User is FullAdmin
    private bool IsFullAdmin()
    {
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        return role == Role.FullAdmin.ToString(); 
    }

    //  Register new User just FullAdmin can that

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!IsFullAdmin())
        {
            return Forbid("Only FullAdmin can create users."); //  Restrict to FullAdmin
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role
        };

        await _userRepository.AddAsync(user);
        return Ok(new { message = "User registered successfully!", user });
    }


    // Get the current logged-in user's username and role

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;
        var currentRole = User.FindFirst(ClaimTypes.Role)?.Value;

        // Only FullAdmin can access this endpoint
        if (currentRole != "FullAdmin")
        {
            return Forbid(); // Return 403 Forbidden
        }

        // Get all users from the database
        var users = await _userRepository.GetAllAsync();

        // Exclude the currently logged-in user
        var filteredUsers = users
            .Where(user => user.Username != currentUsername)
            .Select(user => new AllUsersDto(user.Username, user.Role.ToString(), user.IsBanned))
            .ToList();

        return Ok(filteredUsers);
    }


    // Delete user by username just FullAdmin can that


    [HttpDelete("delete/{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        if (!IsFullAdmin())
        {
            return Forbid("Only FullAdmin can delete users."); // Restrict to FullAdmin
        }

        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null) return NotFound(new { message = $"User '{username}' not found." });

        await _userRepository.DeleteAsync(user);
        return Ok(new { message = $"User '{username}' deleted successfully." });
    }

    // Ban user by username just FullAdmin can that


    [HttpPost("ban/{username}")]
    public async Task<IActionResult> BanUser(string username)
    {
        if (!IsFullAdmin())
        {
            return Forbid("Only FullAdmin can ban users."); //  Restrict to FullAdmin
        }

        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null) return NotFound(new { message = $"User '{username}' not found." });

        user.IsBanned = true;
        await _userRepository.SaveChangesAsync();
        return Ok(new { message = $"User '{username}' has been banned." });
    }

    // Unban user by username just FullAdmin can that

    [HttpPost("unban/{username}")]
    public async Task<IActionResult> UnbanUser(string username)
    {
        // Get current logged-in user's role
        var currentRole = User.FindFirst(ClaimTypes.Role)?.Value;

        // Only FullAdmin can unban users
        if (currentRole != "FullAdmin")
        {
            return Forbid(); // Return 403 Forbidden
        }

        // Get the user by username
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // If the user is not banned, return a message
        if (!user.IsBanned)
        {
            return BadRequest(new { message = "User is already unbanned" });
        }

        // Unban the user
        user.IsBanned = false;
        await _userRepository.SaveChangesAsync();

        return Ok(new { message = $"User {username} has been unbanned successfully" });
    }
}
