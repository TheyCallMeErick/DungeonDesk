using Bogus;
using DungeonDeskBackend.Application.Fakers;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Data;

public class Seeder
{

    public List<Adventure> Adventures { get; set; } = new List<Adventure>();
    public List<Chronicle> Chronicles { get; set; } = new List<Chronicle>();
    public List<Desk> Desks { get; set; } = new List<Desk>();
    public List<PlayerDesk> PlayerDesks { get; set; } = new List<PlayerDesk>();
    public List<Session> Sessions { get; set; } = new List<Session>();
    public List<User> Users { get; set; } = new List<User>();
    public List<Player> Players { get; set; } = new List<Player>();

    public void Init()
    {
        Randomizer.Seed = new Random(8675309);
        var users = UserFaker.Make().Generate(10);
        var players = PlayerFaker.Make()
    .RuleFor(x => x.User, f => f.PickRandom(users))
    .Generate(users.Count());
        var adventures = AdventureFaker.Make().RuleFor(x => x.Author, f => f.PickRandom(players)).Generate(10);
        var desks = DeskFaker.Make().RuleFor(x => x.Adventure, f => f.PickRandom(adventures)).Generate(10);
        var playerDesks = PlayerDeskFaker.Make().RuleFor(x => x.Player, f => f.PickRandom(players)).RuleFor(x => x.Desk, f => f.PickRandom(desks)).Generate(10);
        var sessions = SessionFaker.Make().RuleFor(x => x.Desk, f => f.PickRandom(desks)).Generate(10);
        var chronicles = ChronicleFaker.Make().RuleFor(x => x.Session, f => f.PickRandom(sessions)).RuleFor(x => x.Author, f => f.PickRandom(players)).Generate(10);
        Adventures = adventures.ToList();
        Chronicles = chronicles.ToList();
        Desks = desks.ToList();
        PlayerDesks = playerDesks.ToList();
        Sessions = sessions.ToList();
        Players = players.ToList();
        Users = users.ToList();
    }

}
