using Bogus;
using Bogus.Generators;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Fakers;

public class ChronicleFaker
{
    public static Faker<Chronicle> Make()
    {
        return new Faker<Chronicle>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.CreatedAt, f => new Date().Past().ToUniversalTime())
            .RuleFor(c => c.Title, f => new Lorem().Sentance(1, 1))
            .RuleFor(c => c.Content, f => new Lorem().Paragraphs(1))
            .RuleFor(c => c.SessionId, f => Guid.NewGuid())
            .RuleFor(c => c.AuthorId, f => Guid.NewGuid());
    }

    public static Chronicle MakeOne() => Make().Generate();
    public static IEnumerable<Chronicle> MakeMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return MakeOne();
        }
    }
}
