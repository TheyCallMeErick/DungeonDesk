namespace DungeonDeskBackend.Application.DTOs.Outputs;

public class PaginationOutputDTO
{
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }

    public PaginationOutputDTO(int totalItems, int totalPages, int currentPage, int pageSize)
    {
        TotalItems = totalItems;
        TotalPages = totalPages;
        CurrentPage = currentPage;
        PageSize = pageSize;
    }
    
    public static PaginationOutputDTO Create(int totalItems, int currentPage, int pageSize)
    {
        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        return new PaginationOutputDTO(totalItems, totalPages, currentPage, pageSize);
    }
}
