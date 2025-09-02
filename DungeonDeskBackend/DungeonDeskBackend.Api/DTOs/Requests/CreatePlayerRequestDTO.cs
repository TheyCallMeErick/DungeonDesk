using System.ComponentModel.DataAnnotations;

namespace DungeonDeskBackend.Api.DTOs.Requests; 

public record CreatePlayerRequestDTO(
    [param:StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    [param:RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
    string Name,
    string Email,
    [param:Required(ErrorMessage = "Password is required.")]
    [param:StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [param:DataType(DataType.Password)]
    string Password,
    [param:Required(ErrorMessage ="Password confirmation is required")]
    [param:StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [param:DataType(DataType.Password)]
    string PasswordConfirmation,
    string Username
); /* : IValidatableObject
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

        if (Password == PasswordConfirmation)
        {
            yield return new ValidationResult("Password and Password Confirmation doesn't match");
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
}; */
