using Bogus;
using Bogus.Generators;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Fakers;

public class SessionFaker
{
    public static Faker<Session> Make()
    {
        return new Faker<Session>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.CreatedAt, f => f.Date.Past().ToUniversalTime())
            .RuleFor(s => s.ScheduledAt, f => f.Date.Future().ToUniversalTime())
            .RuleFor(s => s.StartedAt, f => f.Date.Recent().ToUniversalTime())
            .RuleFor(s => s.EndedAt, (f, s) => s.StartedAt?.AddHours(2).ToUniversalTime())
            .RuleFor(s => s.DeskId, f => Guid.NewGuid())
            .RuleFor(s => s.Notes, f => new Lorem().Sentance());
    }

    public static Session MakeOne() => Make().Generate();

    public static IEnumerable<Session> MakeMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return MakeOne();
        }
    }
}
