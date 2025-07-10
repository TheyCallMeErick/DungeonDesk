using Bogus;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Tests.Fixtures.Fakers; 

public class PlayerDeskFaker
{
    public static Faker<PlayerDesk> Make()
    {
        return new Faker<PlayerDesk>()
            .RuleFor(pd => pd.PlayerId, f => Guid.NewGuid())
            .RuleFor(pd => pd.DeskId, f => Guid.NewGuid())
            .RuleFor(pd => pd.JoinedAt, f => f.Date.Past())
            .RuleFor(pd => pd.Role, f => f.PickRandom(new List<string>{"Jogador", "Narrador", "Convidado"}));
    }

    public static PlayerDesk MakeOne() => Make().Generate();
}
