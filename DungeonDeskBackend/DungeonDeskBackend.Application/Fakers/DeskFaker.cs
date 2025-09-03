using Bogus;
using Bogus.Generators;
using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Fakers;

public class DeskFaker
{
    public static Faker<Desk> Make()
    {
        return new Faker<Desk>()
            .RuleFor(d => d.Id, f => Guid.NewGuid())
            .RuleFor(d => d.CreatedAt, f => new Date().Past().ToUniversalTime())
            .RuleFor(d => d.Name, f => new Lorem().Paragraph(1))
            .RuleFor(d => d.Description, f => new Lorem().Paragraph())
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
