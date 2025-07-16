namespace DungeonDeskBackend.Application.DTOs.Outputs.Auth;

public record UserAuthOutputDTO
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }

    public UserAuthOutputDTO(string token, string refreshToken)
    {
        AccessToken = token;
        RefreshToken = refreshToken;
    }
}
