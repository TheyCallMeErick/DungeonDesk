using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Fakers;
using DungeonDeskBackend.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ObservatorioApi.Auth;

namespace DungeonDeskBackend.Tests;

public class TokenManagerServiceTests : IDisposable
{
    private readonly ITokenManagerService tokenManagerService;
    private readonly DungeonDeskDbContext _dbContext;

    public TokenManagerServiceTests()
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
        tokenManagerService = new TokenManagerService(configuration, _dbContext);
    }

    [Fact]
    public void GenerateAccessToken_ShouldReturnValidToken()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        // Act
        var token = tokenManagerService.GenerateAccessToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public async Task RenewRefreshToken_ShouldReturnNewToken()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        var token = RefreshTokenFaker.MakeOne();
        token.UserId = user.Id;
        token.RevokedAt = null;
        token.IsRevoked = false;
        _dbContext.Users.Add(user);
        _dbContext.RefreshTokens.Add(token);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await tokenManagerService.RenewRefreshToken(user.Id, token.CreatedByIp!, token.DeviceInfo!);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data.AccessToken);
        Assert.NotEqual(token.Token, result.Data.AccessToken);
    }

    [Fact]
    public async Task RenewRefreshToken_ShouldReturnFailure_WhenNoValidToken()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await tokenManagerService.RenewRefreshToken(user.Id, "", "");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No valid refresh token found for the user.", result.Message);
    }

    [Fact]
    public async Task RefreshAccessToken_ShouldReturnNewAccessToken()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        var token = RefreshTokenFaker.MakeOne();
        token.UserId = user.Id;
        token.RevokedAt = null;
        token.IsRevoked = false;
        _dbContext.Users.Add(user);
        _dbContext.RefreshTokens.Add(token);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await tokenManagerService.RefreshAccessToken(token.Token!);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
    }

    [Fact]
    public async Task RefreshAccessToken_ShouldReturnFailure_WhenInvalidToken()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await tokenManagerService.RefreshAccessToken("invalid_token");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid refresh token.", result.Message);
    }

    [Fact]
    public async Task RefreshAccessToken_ShouldReturnFailure_WhenTokenAlreadyUsed()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        var token = RefreshTokenFaker.MakeOne();
        token.UserId = user.Id;
        token.RevokedAt = null;
        token.IsRevoked = false;
        _dbContext.Users.Add(user);
        _dbContext.RefreshTokens.Add(token);
        await _dbContext.SaveChangesAsync();

        // Act
        await tokenManagerService.RefreshAccessToken(token.Token!);
        await tokenManagerService.RenewRefreshToken(user.Id, token.CreatedByIp!, token.DeviceInfo!);
        var result = await tokenManagerService.RefreshAccessToken(token.Token!);
 
        // Assert again
        Assert.False(result.Success);
        Assert.Equal("Invalid or expired refresh token.", result.Message);
    }

    [Fact]
    public async Task RefreshAccessToken_ShouldReturnFailure_WhenTokenIsNullOrEmpty()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await tokenManagerService.RefreshAccessToken("");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Token is null or empty.", result.Message);
    }

    [Fact]
    public async Task RefreshAccessToken_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange 
        var token = RefreshTokenFaker.MakeOne();
        token.UserId = Guid.NewGuid();
        token.RevokedAt = null;
        token.IsRevoked = false;
        _dbContext.RefreshTokens.Add(token);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await tokenManagerService.RefreshAccessToken(token.Token!);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("User associated with the refresh token not found.", result.Message);
    }

    [Fact]
    public async Task RenewRefreshToken_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange 
        var token = RefreshTokenFaker.MakeOne();
        token.UserId = Guid.NewGuid();
        _dbContext.RefreshTokens.Add(token);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await tokenManagerService.RenewRefreshToken(token.UserId, token.CreatedByIp!, token.DeviceInfo!);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No valid refresh token found for the user.", result.Message);
    }

    [Fact]
    public async Task RenewRefreshToken_ShouldReturnFailure_WhenTokenIsRevoked()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        var token = RefreshTokenFaker.MakeOne();
        token.UserId = user.Id;
        token.RevokedAt = DateTime.UtcNow;
        token.IsRevoked = true;
        _dbContext.Users.Add(user);
        _dbContext.RefreshTokens.Add(token);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await tokenManagerService.RenewRefreshToken(user.Id, token.CreatedByIp!, token.DeviceInfo!);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No valid refresh token found for the user.", result.Message);
    }

    [Fact]
    public async Task RenewRefreshToken_ShouldReturnFailure_WhenTokenIsExpired()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        var token = RefreshTokenFaker.MakeOne();
        token.UserId = user.Id;
        token.ExpiresAt = DateTime.UtcNow.AddDays(-1); 
        _dbContext.Users.Add(user);
        _dbContext.RefreshTokens.Add(token);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await tokenManagerService.RenewRefreshToken(user.Id, token.CreatedByIp!, token.DeviceInfo!);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No valid refresh token found for the user.", result.Message);
    }

    [Fact]
    public async Task RenewRefreshToken_ShouldReturnFailure_WhenTokenIsNullOrEmpty()
    {
        // Arrange 
        var user = UserFaker.MakeOne();
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await tokenManagerService.RenewRefreshToken(user.Id, "", "");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No valid refresh token found for the user.", result.Message);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
