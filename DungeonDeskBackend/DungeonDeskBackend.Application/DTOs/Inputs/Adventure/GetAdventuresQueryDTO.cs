namespace DungeonDeskBackend.Application.DTOs.Inputs.Adventure;

public record GetAdventuresQueryDTO
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public Guid? Author { get;init; }
}
