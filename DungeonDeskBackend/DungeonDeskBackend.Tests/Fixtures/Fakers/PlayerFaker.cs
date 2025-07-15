using Bogus;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Tests.Fixtures.Fakers;

public class PlayerFaker
{
    public static Faker<Player> Make()
    {
        return new Faker<Player>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.CreatedAt, f => f.Date.Past());
    }

    public static Player MakeOne() => Make().Generate();

    public static IEnumerable<Player> MakeMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return MakeOne();
        }
    }
}
