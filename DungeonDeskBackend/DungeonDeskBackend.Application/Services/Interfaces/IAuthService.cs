using DungeonDeskBackend.Application.DTOs.Inputs.Auth;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.DTOs.Outputs.Auth;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IAuthService
{
    Task<OperationResultDTO<string>> RefreshAccessTokenAsync(string refreshToken);
    Task<OperationResultDTO<UserAuthOutputDTO>> ValidateUserCredentialsAsync(UserCredentialsInputDTO dto);
    Task<OperationResultDTO<UserAuthOutputDTO>> RefreshAccessTokenAsync(Guid refreshToken, string ipAddress, string deviceInfo);
}
