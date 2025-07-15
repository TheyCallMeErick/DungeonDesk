namespace DungeonDeskBackend.Application.DTOs.Outputs.Auth;

public record UserAuthOutputDTO
{
    public string Token { get; init; }
    public string RefreshToken { get; init; }

    public UserAuthOutputDTO(string token, string refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }
}
