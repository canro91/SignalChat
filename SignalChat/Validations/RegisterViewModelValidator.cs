using FluentValidation;
using SignalChat.Models;

namespace SignalChat.Validations;

public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.Username)
           .NotEmpty()
           .WithMessage("Username is required.")
           .MinimumLength(5)
           .WithMessage("Username must be at least 5 characters long.")
           .MaximumLength(256)
           .WithMessage("Username cannot exceed 256 characters.")
           .Matches("^[a-zA-Z_-]+$")
           .WithMessage("Username can contain letters, underscores, and hyphens only.");

        RuleFor(x => x.Password)
           .NotEmpty()
           .WithMessage("Password is required.")
           .MinimumLength(8)
           .WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.ConfirmPassword)
           .NotEmpty()
           .WithMessage("Confirm password is required.")
           .MinimumLength(8)
           .WithMessage("Confirm password must be at least 8 characters long.")
           .Equal(x => x.Password)
           .WithMessage("Passwords do not match.");
    }
}