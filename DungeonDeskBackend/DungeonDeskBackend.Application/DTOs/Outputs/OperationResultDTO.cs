namespace DungeonDeskBackend.Application.DTOs.Outputs;

public record OperationResultDTO<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public PaginationOutputDTO? Pagination { get; init; }

    private OperationResultDTO(bool success)
    {
        Success = success;
    }

    private OperationResultDTO(bool success, string? message)
    {
        Success = success;
        Message = message;
    }

    public static OperationResultDTO<T> SuccessResult()
    {
        return new OperationResultDTO<T>(true);
    }

    public static OperationResultDTO<T> FailureResult(string message)
    {
        return new OperationResultDTO<T>(false, message);
    }

    public OperationResultDTO<T> WithMessage(string? message)
    {
        return this with { Message = message };
    }

    public OperationResultDTO<T> WithData(T? data)
    {
        return this with
        {
            Data = data
        };
    }

    public OperationResultDTO<T> WithPagination(PaginationOutputDTO pagination)
    {
        return this with
        {
            Pagination = pagination
        };
    }
}
