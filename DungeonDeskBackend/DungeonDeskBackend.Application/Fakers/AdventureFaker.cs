using Bogus;
using Bogus.Generators;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Fakers;

public class AdventureFaker
{
    public static Faker<Adventure> Make()
    {
        return new Faker<Adventure>()
            .RuleFor(a => a.Id, f => Guid.NewGuid())
            .RuleFor(a => a.CreatedAt, f => new Date().Past())
            .RuleFor(a => a.Title, f => new Lorem().Sentance())
            .RuleFor(a => a.Description, f => new Lorem().Paragraph())
            .RuleFor(a => a.AuthorId, f => Guid.NewGuid());
    }

    public static Adventure MakeOne() => Make().Generate();
    public static IEnumerable<Adventure> MakeMany(int count) {
        for (int i = 0; i < count; i++)
        {
            yield return MakeOne();
        }
    }
}
