using Bogus;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Tests.Fixtures.Fakers; 

public class ChronicleFaker
{
   public static Faker<Chronicle> Make()
    {
        return new Faker<Chronicle>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(c => c.CreatedAt, f => f.Date.Past())
            .RuleFor(c => c.Title, f => f.Lorem.Sentance(2,4))
            .RuleFor(c => c.Content, f => f.Lorem.Paragraphs(1))
            .RuleFor(c => c.SessionId, f => Guid.NewGuid())
            .RuleFor(c => c.AuthorId, f => Guid.NewGuid());
    }

    public static Chronicle MakeOne() => Make().Generate();
}
