namespace DungeonDeskBackend.Domain.Validations;

public record ValidationResult
{
    public bool IsValid { get; init; }
    public string Message { get; init; } = string.Empty;

    public ValidationResult(bool isValid, string message = "")
    {
        IsValid = isValid;
        Message = message;
    }

    public static ValidationResult Success() => new(true);
    public static ValidationResult Failure(string message) => new(false, message);
}
