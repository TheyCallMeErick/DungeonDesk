namespace DungeonDeskBackend.Domain.Validations.Interfaces;

public interface IValidationRule<T>
{
    ValidationResult Validate(T dto);
}
