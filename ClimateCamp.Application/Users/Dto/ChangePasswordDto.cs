using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace ClimateCamp.Common.Users.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }

    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Password is required.")
                .Length(4, 32)
                .WithMessage("Password must contain between 4 and 32 characters.");
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Password is required.")
                .Length(4, 32)
                .WithMessage("Password must contain between 4 and 32 characters.");
        }
    }

}
