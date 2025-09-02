using Bogus;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Fakers; 

public class RefreshTokenFaker
{
      public static Faker<RefreshToken> Make()
    {
        return new Faker<RefreshToken>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.CreatedAt, f => f.Date.Past())
            .RuleFor(p => p.ExpiresAt, f => f.Date.Future())
            .RuleFor(p => p.Token, f => string.Join("-", f.Lorem.Words(3)))
            .RuleFor(p => p.IsRevoked, f => f.Random.Bool())
            .RuleFor(p => p.RevokedAt, f => f.Date.Past())
            .RuleFor(p => p.CreatedByIp, f => f.Internet.Ip())
            .RuleFor(p => p.DeviceInfo, f => f.Lorem.Sentance(2, 5));
    }

    public static RefreshToken MakeOne() => Make().Generate();

    public static IEnumerable<RefreshToken> MakeMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return MakeOne();
        }
    }
}
