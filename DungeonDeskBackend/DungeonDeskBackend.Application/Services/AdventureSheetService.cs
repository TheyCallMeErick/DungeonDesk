using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Inputs.SheetTemplate;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Services;

public class AdventureSheetService
{
    private readonly DungeonDeskDbContext context;

    public AdventureSheetService(DungeonDeskDbContext context)
    {
        this.context = context;
    }

    public async Task<OperationResultDTO<Sheet>> CreateAdventureSheetTemplateAsync(CreateSheetTemplateDTO dto)
    {
        var adventure = await context.Adventures
             .Include(a => a.AdventureSheetTemplates)
             .FirstOrDefaultAsync(a => a.Id == dto.AdventureId);
        if (adventure == null)
        {
            return OperationResultDTO<Sheet>
                .FailureResult($"Adventure with ID {dto.AdventureId} not found.");
        }
        var version = adventure.AdventureSheetTemplates.Count + 1;
        var template = new AdventureSheetTemplate
        {
            Title = dto.Title,
            Description = dto.Description,
            AdventureId = dto.AdventureId,
            Version = version
        };
        context.AdventureSheetTemplates.Add(template);
        await context.SaveChangesAsync();
        return OperationResultDTO<Sheet>.SuccessResult();
    }
}
