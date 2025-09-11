using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Inputs.Auth;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.DTOs.Outputs.Auth;
using DungeonDeskBackend.Application.DTOs.Outputs.User;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Services;

public class AuthService : IAuthService
{
    private readonly DungeonDeskDbContext _context;
    private readonly ITokenManagerService _tokenManager;

    public AuthService(DungeonDeskDbContext context, ITokenManagerService tokenManager)
    {
        _context = context;
        _tokenManager = tokenManager;
    }

    public async Task<OperationResultDTO<string>> RefreshAccessTokenAsync(string refreshToken)
    {
        var result = await _tokenManager.RefreshAccessToken(refreshToken);
        if (!result.Success)
        {
            return OperationResultDTO<string>.FailureResult(result.Message ?? "Failed to refresh token.");
        }
        return OperationResultDTO<string>
            .SuccessResult()
            .WithData(result.Data);
    }

    public async Task<OperationResultDTO<UserAuthOutputDTO>> ValidateUserCredentialsAsync(UserCredentialsInputDTO dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
        if (user == null)
        {
            return OperationResultDTO<UserAuthOutputDTO>
                .FailureResult("Invalid email or password.");
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!isPasswordValid)
        {
            return OperationResultDTO<UserAuthOutputDTO>
                .FailureResult("Invalid email or password.");
        }

        var accessToken = _tokenManager.GenerateAccessToken(user);
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            CreatedByIp = dto.Ip,
            DeviceInfo = dto.DeviceInfo
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return OperationResultDTO<UserAuthOutputDTO>
            .SuccessResult()
            .WithData(new UserAuthOutputDTO
            (
                token: accessToken,
                refreshToken: refreshToken.Token
            ));
    }

    public async Task<OperationResultDTO<UserAuthOutputDTO>> RefreshAccessTokenAsync(Guid refreshToken, string ipAddress, string deviceInfo)
    {
        var result = await _tokenManager.RenewRefreshToken(refreshToken, ipAddress, deviceInfo);
        if (!result.Success)
        {
            return OperationResultDTO<UserAuthOutputDTO>.FailureResult(result.Message ?? "Failed to refresh token.");
        }
        return OperationResultDTO<UserAuthOutputDTO>
            .SuccessResult()
            .WithData(result.Data);
    }

    public async Task<OperationResultDTO<UserOutputDTO>> GetCurrentUserAsync(string userId)
    {
        var result = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if (result == null)
        {
            return OperationResultDTO<UserOutputDTO>.FailureResult("Failed to get user from token.");
        }
        return OperationResultDTO<UserOutputDTO>
            .SuccessResult()
            .WithData(new UserOutputDTO
            (
                Id : result.Id,
                Username : result.Username,
                Name : result.Name,
                Email : result.Email,
                ProfilePictureFileName : result.ProfilePictureFileName
            ));
    }
}
