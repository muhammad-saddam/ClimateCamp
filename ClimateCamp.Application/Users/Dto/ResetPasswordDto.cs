using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace ClimateCamp.Common.Users.Dto
{
    public class ResetPasswordDto
    {
        [Required]
        public string EmailAddress { get; set; }

    }

    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email address is required.")
                .MaximumLength(256)
                .WithMessage("An email address may contain up to 256 characters.");
        }
    }
}
