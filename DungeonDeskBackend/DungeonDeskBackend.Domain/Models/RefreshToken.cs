namespace DungeonDeskBackend.Domain.Models; 

public class RefreshToken : BaseModel
{
    public string? Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime? RevokedAt { get; set; }
    public RefreshToken? ReplacedByToken { get; set; }


    public string? CreatedByIp { get; set; }
    public string? DeviceInfo { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}
