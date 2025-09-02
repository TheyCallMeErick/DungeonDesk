using Bogus;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Fakers; 

public class UserFaker
{
    public static Faker<User> Make()
    {
        return new Faker<User>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.CreatedAt, f => f.Date.Past())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.Password, f => "")
            .RuleFor(p => p.Name, f => f.Person.Name)
            .RuleFor(p => p.Username, f => f.Internet.UserName());
    }

    public static User MakeOne() => Make().Generate();

    public static IEnumerable<User> MakeMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return MakeOne();
        }
    }
}
