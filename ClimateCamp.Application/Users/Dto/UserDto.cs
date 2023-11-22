using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using ClimateCamp.Common.Authorization.Roles;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Common.Validation;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClimateCamp.Common.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        public string Phone { get; set; }
        public Guid? OrganizationId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Surname { get; set; }
        [Required]
        [StringLength(AbpUserBase.MaxPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }
        public string? ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string[] RoleNames { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        
        /// <summary>
        /// TODO: the ROle is an entity type, and is used here in a DTO type, it's architecturay wrong. Instead create a MapProfile type tomap from the entity type Role to specific propertis of the DTO like ROleId and ROleName
        /// </summary>
        public Role Role { get; set; }

        public bool IsFirstLoginExperience { get; set; }
        public bool IsSelfServiceUser { get; set; } = false;

        /// <summary>
        /// TODO: the Organization is an entity type, and is used here in a DTO type, instead we should use OrganizationDto
        /// </summary>
        public Core.Organization? Organization { get; set; }

        /// <summary>
        /// TODO: seem unused, improperly named as plural. 
        /// </summary>
        public Abp.Application.Editions.Edition? Editions { get; set; }
    }

    public class UserDtoValidator : AbstractValidator<UserDto>
    {

        public UserDtoValidator()
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

            //Some issue was happening in the self service registration regarding the phone control need to check it later
            
            //RuleFor(x => x.Phone)
            //    .Must((o, Phone) => ValidationHelper.IsValidPhone(Phone))
            //    .WithMessage("Phone number must be valid for your selected country.");
        }

    }
}
