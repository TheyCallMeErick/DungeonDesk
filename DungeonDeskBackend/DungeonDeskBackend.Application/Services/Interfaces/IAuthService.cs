using DungeonDeskBackend.Application.DTOs.Inputs.Auth;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.DTOs.Outputs.Auth;
using DungeonDeskBackend.Application.DTOs.Outputs.User;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IAuthService
{
    Task<OperationResultDTO<string>> RefreshAccessTokenAsync(string refreshToken);
    Task<OperationResultDTO<UserAuthOutputDTO>> ValidateUserCredentialsAsync(UserCredentialsInputDTO dto);
    Task<OperationResultDTO<UserAuthOutputDTO>> RefreshAccessTokenAsync(Guid refreshToken, string ipAddress, string deviceInfo);
    Task<OperationResultDTO<UserOutputDTO>> GetCurrentUserAsync(string userId);
}
