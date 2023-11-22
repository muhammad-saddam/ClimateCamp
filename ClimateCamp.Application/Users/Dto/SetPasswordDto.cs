using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClimateCamp.Users.Dto
{
    public class SetPasswordDto
    {

        [Required]
        public Guid OrganizationId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class SetPasswordDtoValidator : AbstractValidator<SetPasswordDto>
    {
        public SetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email address is required.")
                .MaximumLength(256)
                .WithMessage("An email address may contain up to 256 characters.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .Length(4, 32)
                .WithMessage("Password must contain between 4 and 32 characters.");
        }
    }
}
