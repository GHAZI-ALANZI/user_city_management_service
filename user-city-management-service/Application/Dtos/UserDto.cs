using System.ComponentModel.DataAnnotations;
using user_city_management_service.Domain.Enums;

namespace user_city_management_service.Application.Dtos;

public record UserDto(string Username, string Email, string Role);
public record RegisterRequest(
    [Required] string Username,
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password,
    [Required] Role Role 
); 
public record LoginRequest(string Username, string Password);
