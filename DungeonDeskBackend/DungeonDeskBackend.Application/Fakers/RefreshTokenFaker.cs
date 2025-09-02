using Bogus;
using Bogus.Generators;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Fakers; 

public class RefreshTokenFaker
{
      public static Faker<RefreshToken> Make()
    {
        return new Faker<RefreshToken>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.CreatedAt, f => new Date().Past())
            .RuleFor(p => p.ExpiresAt, f => new Date().Future())
            .RuleFor(p => p.Token, f => string.Join("-", new Lorem().Words(3)))
            .RuleFor(p => p.IsRevoked, f => f.Random.Bool())
            .RuleFor(p => p.RevokedAt, f => new Date().Past())
            .RuleFor(p => p.CreatedByIp, f => new Internet().Ip())
            .RuleFor(p => p.DeviceInfo, f => new Lorem().Sentance(2, 5));
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
