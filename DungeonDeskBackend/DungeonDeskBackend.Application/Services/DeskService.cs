using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Desk;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.DTOs.Outputs.Desk;
using DungeonDeskBackend.Application.Repositories.Interfaces;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Models;
using DungeonDeskBackend.Domain.Validations;
using DungeonDeskBackend.Domain.Validations.DTOs;
using DungeonDeskBackend.Domain.Validations.Rules;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Services;

public class DeskService : IDeskService
{
    private readonly DungeonDeskDbContext _context;
    private readonly IDeskRepository _deskRepository;

    public DeskService(DungeonDeskDbContext context, IDeskRepository deskRepository)
    {
        _context = context;
        _deskRepository = deskRepository;
    }

    public async Task<OperationResultDTO<DeskOutputDTO>> CreateDeskAsync(CreateDeskInputDTO desk)
    {
        var master = await _context.Players.FindAsync(desk.MasterId);
        if (master == null)
        {
            return OperationResultDTO<DeskOutputDTO>
                .FailureResult($"Master with ID {desk.MasterId} not found.");
        }
        var adventure = await _context.Adventures.FindAsync(desk.AdventureId);
        if (adventure == null)
        {
            return OperationResultDTO<DeskOutputDTO>
                .FailureResult($"Adventure with ID {desk.AdventureId} not found.");
        }
        var newDesk = new Desk
        {
            Name = desk.Name,
            Description = desk.Description,
            MaxPlayers = desk.MaxPlayers,
            AdventureId = adventure.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Status = ETableStatus.Open
        };
        newDesk.PlayerDesks.Add(new PlayerDesk
        {
            PlayerId = master.Id,
            Desk = newDesk,
            Role = EPlayerDeskRole.DeskMaster
        });
        await _context.Desks.AddAsync(newDesk);
        await _context.SaveChangesAsync();
        return OperationResultDTO<DeskOutputDTO>
            .SuccessResult()
            .WithData(new DeskOutputDTO (
                Id: newDesk.Id,
                Name: newDesk.Name,
                Description: newDesk.Description,
                AdventureId: newDesk.AdventureId,
                MaxPlayers: newDesk.MaxPlayers,
                Status: newDesk.Status
            ))
            .WithMessage("Desk created successfully.");
    }

    public async Task<OperationResultDTO<Desk>> DeleteDeskAsync(Guid id)
    {
        var desk = _context.Desks.Find(id);
        if (desk == null)
        {
            return OperationResultDTO<Desk>
                .FailureResult($"Desk with ID {id} not found.");
        }

        _context.Desks.Remove(desk);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Desk>
            .SuccessResult()
            .WithMessage("Desk deleted successfully.");
    }

    public async Task<OperationResultDTO<DeskOutputDTO>> GetDeskByIdAsync(Guid id)
    {
        var desk = await _context.Desks.FirstOrDefaultAsync(x => x.Id == id);
        if (desk == null)
        {
            return OperationResultDTO<DeskOutputDTO>
                .FailureResult($"Desk with ID {id} not found.");
        }

        return OperationResultDTO<DeskOutputDTO>
            .SuccessResult()
            .WithData(new DeskOutputDTO
            (
                Id: desk.Id,
                Name: desk.Name,
                Description: desk.Description,
                AdventureId: desk.AdventureId,
                MaxPlayers: desk.MaxPlayers,
                Status: desk.Status
            ))
            .WithMessage("Desk retrieved successfully.");
    }

    public async Task<OperationResultDTO<IEnumerable<DeskOutputDTO>>> GetDesksAsync(QueryInputDTO<GetDesksQueryDTO> queryInput)
    {
        var data = await _deskRepository.GetDesksAsync(queryInput);
        return OperationResultDTO<IEnumerable<DeskOutputDTO>>
            .SuccessResult()
            .WithData(data.ToList().ConvertAll(desk => new DeskOutputDTO
            (
                Id: desk.Id,
                Name: desk.Name,
                Description: desk.Description,
                AdventureId: desk.AdventureId,
                MaxPlayers: desk.MaxPlayers,
                Status: desk.Status
            )))
            .WithMessage("Desks retrieved successfully.")
            .WithPagination(PaginationOutputDTO.Create(data.Count(), 1, 10));
    }

    public async Task<OperationResultDTO<IEnumerable<Player>>> GetPlayersByDeskIdAsync(Guid deskId)
    {
        var players = await _context
            .Players
            .Where(p => p.PlayerDesks.Any(d => d.DeskId == deskId))
            .ToListAsync();
        if (players == null || !players.Any())
        {
            return OperationResultDTO<IEnumerable<Player>>
                .FailureResult($"No players found for desk with ID {deskId}.");
        }
        return OperationResultDTO<IEnumerable<Player>>
            .SuccessResult()
            .WithData(players)
            .WithMessage("Players retrieved successfully.")
            .WithPagination(PaginationOutputDTO.Create(players.Count, 1, 10));
    }

    public async Task<OperationResultDTO<Desk>> UpdateDeskAsync(UpdateDeskInputDTO dto)
    {
        var existingDesk = await _context.Desks.FirstOrDefaultAsync(x => x.Id == dto.DeskId);
        if (existingDesk == null)
        {
            return OperationResultDTO<Desk>
                .FailureResult($"Desk with ID {dto.DeskId} not found.");
        }
        var validationResult = Validator.Validate(
            dto: new OnlyTheDeskMasterShoudManageDeskDTO
            (
                Desk : existingDesk,
                PlayerTryingUpdateId : dto.MasterId
            ),
            rule: new OnlyTheDeskMasterShoudManageDesk()
        );
        if( !validationResult.IsValid)
        {
            return OperationResultDTO<Desk>
                .FailureResult(validationResult.Message);
        }
        existingDesk.Name = dto.Name;
        existingDesk.Description = dto.Description;
        existingDesk.UpdatedAt = DateTime.UtcNow;

        _context.Desks.Update(existingDesk);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Desk>
            .SuccessResult()
            .WithData(existingDesk)
            .WithMessage("Desk updated successfully.");
    }
}
