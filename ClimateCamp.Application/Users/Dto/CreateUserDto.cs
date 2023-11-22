using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Common.Validation;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(User))]
    public class CreateUserDto : IShouldNormalize
    {
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Surname { get; set; }
        public string Phone { get; set; }
        //public string CountryCode { get; set; }
        public Guid OrganizationId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string? PictureName { get; set; }
        public string? PicturePath { get; set; }

        public Core.Organization Organization { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }

        public string Password { get; set; }
        public bool IsFirstLoginExperience { get; set; }
        public bool IsSelfServiceUser { get; set; } = false;
        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {

        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("First Name is required.")
                .Length(2, 32)
                .WithMessage("First Name length must be between 2 and 32 characters.");
            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage("Last Name is required.")
                .Length(2, 32)
                .WithMessage("Last Name length must be between 2 and 32 characters.");
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email address is required.")
                .MaximumLength(256)
                .WithMessage("Email address may contain up to 256 characters.");
            RuleFor(x => x.Phone)
                 .Must((o, Phone) => ValidationHelper.IsValidPhone(Phone))
                 .WithMessage("\nPhone number must be valid for your selected country.");
        }

    }
}
