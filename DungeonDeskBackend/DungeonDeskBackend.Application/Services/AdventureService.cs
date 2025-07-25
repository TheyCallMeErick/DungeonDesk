using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Adventure;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.Repositories.Interfaces;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Services;

public class AdventureService : IAdventureService
{
    private readonly DungeonDeskDbContext _context;
    private readonly IAdventureRepository _adventureRepository;

    public AdventureService(DungeonDeskDbContext context, IAdventureRepository adventureRepository)
    {
        _context = context;
        _adventureRepository = adventureRepository;
    }

    public async Task<OperationResultDTO<IEnumerable<Adventure>>> GetAdventuresAsync(QueryInputDTO<GetAdventuresQueryDTO> queryInput)
    {
        var data = await  _adventureRepository.GetAdventuresAsync(queryInput);
        if (data == null || !data.Any())
        {
            return OperationResultDTO<IEnumerable<Adventure>>
                .FailureResult("No adventures found.");
        }
        return OperationResultDTO<IEnumerable<Adventure>>
            .SuccessResult()
            .WithData(data)
            .WithMessage("Adventures retrieved successfully.")
            .WithPagination(PaginationOutputDTO.Create(data.ToList().Count, 1, 10));
    }
    public async Task<OperationResultDTO<Adventure>> GetAdventureByIdAsync(Guid adventureId)
    {
        var adventure = await _context.Adventures.FindAsync(adventureId);
        if (adventure == null)
        {
            return OperationResultDTO<Adventure>
                .FailureResult($"Adventure with ID {adventureId} not found.");
        }
        return OperationResultDTO<Adventure>
            .SuccessResult()
            .WithData(adventure)
            .WithMessage("Adventure retrieved successfully.");
    }
    public async Task<OperationResultDTO<Adventure>> CreateAdventureAsync(CreateAdventureInputDTO adventure)
    {
        if (adventure == null)
        {
            return OperationResultDTO<Adventure>
                .FailureResult("Adventure cannot be null");
        }
        var data = new Adventure
        {
            Id = Guid.NewGuid(),
            Title = adventure.Title,
            Description = adventure.Description,
            AuthorId = adventure.AuthorId
        };
        await _context.Adventures.AddAsync(data);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Adventure>
            .SuccessResult()
            .WithData(data)
            .WithMessage("Adventure created successfully.");
    }

    public async Task<OperationResultDTO<IEnumerable<Desk>>> GetDesksUsingAdventureAsync(Guid adventureId)
    {
        var adventure = await _context.Adventures
            .Include(a => a.DesksUsingThis)
            .FirstOrDefaultAsync(a => a.Id == adventureId);

        if (adventure == null)
        {
            return OperationResultDTO<IEnumerable<Desk>>
                .FailureResult($"Adventure with ID {adventureId} not found.");
        }

        return OperationResultDTO<IEnumerable<Desk>>
            .SuccessResult()
            .WithData(adventure.DesksUsingThis)
            .WithMessage("Desks using this adventure retrieved successfully.");
    }

    public async Task<OperationResultDTO<Adventure>> UpdateAdventureAsync(UpdateAdventureInputDTO dto)
    {
        if (dto == null)
        {
            return OperationResultDTO<Adventure>
                .FailureResult("Adventure cannot be null");
        }

        var existingAdventure = await _context.Adventures.FindAsync(dto.AdventureId);

        if (existingAdventure == null)
        {
            return OperationResultDTO<Adventure>
                .FailureResult($"Adventure with ID {dto.AdventureId} not found.");
        }

        if(dto.UserId != existingAdventure.AuthorId)
        {
            return OperationResultDTO<Adventure>
                .FailureResult("You do not have permission to update this adventure.");
        }

        existingAdventure.Title = dto.Title;
        existingAdventure.Description = dto.Description;
        _context.Adventures.Update(existingAdventure);
        await _context.SaveChangesAsync();

        return OperationResultDTO<Adventure>
            .SuccessResult()
            .WithData(existingAdventure)
            .WithMessage("Adventure updated successfully.");
    }

    public Task<OperationResultDTO<Adventure>> DeleteAdventureAsync(Guid adventureId)
    {
        var adventure = _context.Adventures.FirstOrDefault(x => x.Id == adventureId);
        if (adventure == null)
        {
            return Task.FromResult(OperationResultDTO<Adventure>
                .FailureResult($"Adventure with ID {adventureId} not found."));
        }

        _context.Adventures.Remove(adventure);
        _context.SaveChangesAsync();
        return Task.FromResult(OperationResultDTO<Adventure>
            .SuccessResult()
            .WithMessage("Adventure deleted successfully.")
            .WithData(adventure));
    }
}
