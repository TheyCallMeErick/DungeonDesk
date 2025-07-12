using Bogus;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Tests.Fixtures.Fakers;

public class AdventureFaker
{
    public static Faker<Adventure> Make()
    {
        return new Faker<Adventure>()
            .RuleFor(a => a.Id, f => Guid.NewGuid())
            .RuleFor(a => a.CreatedAt, f => f.Date.Past())
            .RuleFor(a => a.Title, f => f.Lorem.Sentance())
            .RuleFor(a => a.Description, f => f.Lorem.Paragraph())
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
