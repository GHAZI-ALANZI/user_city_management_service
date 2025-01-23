using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using user_city_management_service.WebAPI.Controllers;
using user_city_management_service.Application.Services;
using user_city_management_service.Application.Dtos;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>(); 
        _controller = new AuthController(_mockAuthService.Object); // Inject the mocked service
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        var request = new LoginRequest("testuser", "Test@123");
        var token = "valid-jwt-token";

        _mockAuthService.Setup(service => service.AuthenticateAsync(request.Username, request.Password))
            .ReturnsAsync(token); // Simulate successful authentication

        var result = await _controller.Login(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var request = new LoginRequest("testuser", "wrongpassword");

        _mockAuthService.Setup(service => service.AuthenticateAsync(request.Username, request.Password))
            .ReturnsAsync((string)null); //  Simulate failed authentication

        var result = await _controller.Login(request);

        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal(401, unauthorizedResult.StatusCode);
    }
}
