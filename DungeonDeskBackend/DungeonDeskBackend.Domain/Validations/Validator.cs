using DungeonDeskBackend.Domain.Validations.Interfaces;

namespace DungeonDeskBackend.Domain.Validations;

public static class Validator
{
    public static ValidationResult Validate<T>(T dto, IValidationRule<T> rule)
    {
        var result = rule.Validate(dto);
        if (!result.IsValid)
        {
            return result;
        }
        return ValidationResult.Success();
    }

    public static ValidationResult Validate<T>(T dto, IEnumerable<IValidationRule<T>> rules)
    {
        foreach (var rule in rules)
        {
            var result = rule.Validate(dto);
            if (!result.IsValid)
            {
                return result;
            }
        }
        return ValidationResult.Success();
    }

    public static ValidationResult Validate<T>(T dto, params IValidationRule<T>[] rules)
    {
        return Validate(dto, (IEnumerable<IValidationRule<T>>)rules);
    }

    public static ValidationResult Validate<T>(T dto, Func<T, ValidationResult> customValidation)
    {
        var result = customValidation(dto);
        if (!result.IsValid)
        {
            return result;
        }
        return ValidationResult.Success();
    }
}
