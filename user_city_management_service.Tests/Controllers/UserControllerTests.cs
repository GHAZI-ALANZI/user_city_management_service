using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using user_city_management_service.WebAPI.Controllers;
using user_city_management_service.Infrastructure.Repositories;
using user_city_management_service.Application.Dtos;
using user_city_management_service.Domain.Entities;
using user_city_management_service.Domain.Enums;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

public class UserControllerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _controller = new UserController(_mockUserRepository.Object);
    }

    private void SetUserRole(string role)
    {
        var identity = new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim(ClaimTypes.Role, role) 
    }, "mock");

        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext { User = principal }
        };
    }


    //Test RegisterUser fall User With FullAdmin Role return ==> Ok

    [Fact]
    public async Task Register_UserWithFullAdminRole_ReturnsOk()
    {
        SetUserRole("FullAdmin"); 

        var roleEnum = Role.FullAdmin;
        var request = new RegisterRequest("testuser", "test@example.com", "Password123", roleEnum);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = "hashed",
            Role = roleEnum
        };

        _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>())).ReturnsAsync(user);

        var result = await _controller.Register(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }


    //Test RegisterUser fall User Without FullAdmin Role return ==> Forbidden

    [Fact]
    public async Task Register_UserWithoutFullAdminRole_ReturnsForbidden()
    {
        SetUserRole("Admin"); // Admin cannot create users

        var request = new RegisterRequest("testuser", "test@example.com", "Password123", Role.Admin);

        var result = await _controller.Register(request);

        Assert.IsType<ForbidResult>(result);
    }


    //Test DelUser fall User ready Exist  return ==> Ok

    [Fact]
    public async Task DeleteUser_UserExists_ReturnsOk()
    {
        SetUserRole("FullAdmin"); 

        var username = "testuser";
        var user = new User { Username = username, Role = Role.Admin };

        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);
        _mockUserRepository.Setup(repo => repo.DeleteAsync(user)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteUser(username);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }


    //Test DelUser fall User DoesNot Exist  return ==> NotFound

    [Fact]
    public async Task DeleteUser_UserDoesNotExist_ReturnsNotFound()
    {
        SetUserRole("FullAdmin"); 

        var username = "nonexistent";

        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync((User)null);

        var result = await _controller.DeleteUser(username);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); 
        Assert.Equal(404, notFoundResult.StatusCode);
    }


    //Test BanUser fall User ready Exist  return ==> Ok

    [Fact]
    public async Task BanUser_UserExists_ReturnsOk()
    {
        SetUserRole("FullAdmin"); 

        var username = "testuser";
        var user = new User { Username = username, Role = Role.Admin, IsBanned = false };

        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);
        _mockUserRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _controller.BanUser(username);

        var okResult = Assert.IsType<OkObjectResult>(result); 
        Assert.Equal(200, okResult.StatusCode);
    }



    //Test BanUser fall User DoesNot Exist  return ==> NotFound

    [Fact]
    public async Task BanUser_UserDoesNotExist_ReturnsNotFound()
    {
        SetUserRole("FullAdmin");

        var username = "nonexistent";

        _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync((User)null);

        var result = await _controller.BanUser(username);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}
