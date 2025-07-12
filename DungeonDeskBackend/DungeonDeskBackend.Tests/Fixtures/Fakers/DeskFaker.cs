using Bogus;
using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Tests.Fixtures.Fakers;

public class DeskFaker
{
    public static Faker<Desk> Make()
    {
        return new Faker<Desk>()
            .RuleFor(d => d.Id, f => Guid.NewGuid())
            .RuleFor(d => d.CreatedAt, f => f.Date.Past())
            .RuleFor(d => d.Name, f => f.Lorem.Paragraph(1))
            .RuleFor(d => d.Description, f => f.Lorem.Paragraph())
            .RuleFor(d => d.Status, f => f.PickRandom<ETableStatus>())
            .RuleFor(d => d.MaxPlayers, f => f.Random.Number(2, 8))
            .RuleFor(d => d.AdventureId, f => Guid.NewGuid());
    }

    public static Desk MakeOne() => Make().Generate();

    public static IEnumerable<Desk> MakeMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return MakeOne();
        }
    }
}
