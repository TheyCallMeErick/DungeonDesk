

using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.DTOs.Outputs.Auth;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface ITokenManagerService
{
    string GenerateAccessToken(User user);
    Task<OperationResultDTO<UserAuthOutputDTO>> RenewRefreshToken(Guid userId, string ipAddress, string deviceInfo);

    Task<OperationResultDTO<string>> RefreshAccessToken(string token);
}
