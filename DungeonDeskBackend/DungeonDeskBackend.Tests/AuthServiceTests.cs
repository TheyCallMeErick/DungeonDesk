using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Inputs.Auth;
using DungeonDeskBackend.Application.Fakers;
using DungeonDeskBackend.Application.Services;
using DungeonDeskBackend.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ObservatorioApi.Auth;

namespace DungeonDeskBackend.Tests;

public class AuthServiceTests : IDisposable
{
    private readonly IAuthService _authService;
    private readonly DungeonDeskDbContext _dbContext;
    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<DungeonDeskDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "JwtSettings:SecretKey", Guid.NewGuid().ToString() },
                { "JwtSettings:ExpirationTimeInMinutes", "60" },
                { "JwtSettings:Issuer", "issuer" },
                { "JwtSettings:Audience", "audience" }
            })
            .Build();
        _dbContext = new DungeonDeskDbContext(options);
        var tokenManagerService = new TokenManagerService(configuration, _dbContext);

        _authService = new AuthService(_dbContext, tokenManagerService);
    }

    [Fact]
    public async Task AuthenticateUser_ShouldReturnValidToken()
    {
        // Arrange
        var user = UserFaker.MakeOne();
        user.Password = BCrypt.Net.BCrypt.HashPassword("TestPassword");
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _authService.ValidateUserCredentialsAsync(new UserCredentialsInputDTO
        (
            email : user.Email!,
            password : "TestPassword",
            ip : "",
            deviceInfo : "TestDevice"
        ));

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data.AccessToken);
    }

    [Fact]
    public async Task RefreshAccessToken_ShouldReturnNewAccessToken()
    {
        // Arrange
        var user = UserFaker.MakeOne();
        user.Password = BCrypt.Net.BCrypt.HashPassword("TestPassword");
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var authResult = await _authService.ValidateUserCredentialsAsync(new UserCredentialsInputDTO
        (
            email : user.Email!,
            password : "TestPassword",
            ip : "",
            deviceInfo : "TestDevice"
        ));

        Assert.True(authResult.Success);
        Assert.NotNull(authResult.Data);
        Assert.NotEmpty(authResult.Data.AccessToken);

        // Act
        var refreshResult = await _authService.RefreshAccessTokenAsync(authResult.Data.RefreshToken!);

        // Assert
        Assert.True(refreshResult.Success);
        Assert.NotNull(refreshResult.Data);
        Assert.NotEmpty(refreshResult.Data);
    }

    [Fact]
    public async Task RefreshAccessToken_WithInvalidToken_ShouldReturnFailure()
    {
        // Arrange
        var invalidToken = Guid.NewGuid().ToString();

        // Act
        var result = await _authService.RefreshAccessTokenAsync(invalidToken);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid refresh token.", result.Message);
    }

    [Fact]
    public async Task ValidateUserCredentials_WithInvalidCredentials_ShouldReturnFailure()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        user.Password = BCrypt.Net.BCrypt.HashPassword("TestPassword");
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _authService.ValidateUserCredentialsAsync(new UserCredentialsInputDTO
        (
            email: user.Email!,
            password: "WrongPassword",
            ip: "",
            deviceInfo: "TestDevice"
        ));

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid email or password.", result.Message);
        Assert.Null(result.Data);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
