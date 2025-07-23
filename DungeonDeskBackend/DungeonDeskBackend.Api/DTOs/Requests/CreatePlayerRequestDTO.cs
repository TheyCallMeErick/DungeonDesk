using System.ComponentModel.DataAnnotations;

namespace DungeonDeskBackend.Api.DTOs.Requests; 

public record CreatePlayerRequestDTO(
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
    string Name,
    string? Email,
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    string Password,
    string? Username
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {

        if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Username))
        {
            yield return new ValidationResult(
                "Either Email or Username must be provided.",
                new[] { nameof(Email), nameof(Username) }
            );
        }

        if (!string.IsNullOrWhiteSpace(Username))
        {
            if (Username.Length < 3 || Username.Length > 50)
            {
                yield return new ValidationResult("Username must be between 3 and 50 characters long.", new[] { nameof(Username) });
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(Username, @"^[a-zA-Z0-9_]+$"))
            {
                yield return new ValidationResult("Username can only contain letters, numbers, and underscores.", new[] { nameof(Username) });
            }
        }

        if (!string.IsNullOrWhiteSpace(Email))
        {
            if (!new EmailAddressAttribute().IsValid(Email))
            {
                yield return new ValidationResult("Invalid email format.", new[] { nameof(Email) });
            }
        }
    }
};