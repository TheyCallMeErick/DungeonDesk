namespace DungeonDeskBackend.Application.DTOs.Inputs.Auth; 

public record UserCredentialsInputDTO
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string? Ip { get; set; }
    public string? DeviceInfo { get; set; }
    public UserCredentialsInputDTO(string email, string password, string ip, string deviceInfo)
    {
        Email = email;
        Password = password;
        Ip = ip;
        DeviceInfo = deviceInfo;
    }
}
