namespace DungeonDeskBackend.Application.DTOs.Outputs;

public record OperationResultDTO
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public object? Data { get; init; }

    public OperationResultDTO(bool success, string? message = null, object? data = null)
    {
        Success = success;
        Message = message;
        Data = data;
    }
    public static OperationResultDTO SuccessResult(object? data = null, string? message = null)
    {
        return new OperationResultDTO(true, message, data);
    }

    public static OperationResultDTO FailureResult(string message)
    {
        return new OperationResultDTO(false, message);
    }
}
