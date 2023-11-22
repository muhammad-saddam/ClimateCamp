using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Common.Validation;
using ClimateCamp.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class OrganizationUnitDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string VATNumber { get; set; }
        public double MonthlyRevenue { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool IsActive { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? ParentOrganizationUnitId { get; set; }
        public IFormFile File { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public ICollection<OrganizationUnitDto> Children { get; set; }
    }

    public class OrganizationUnitDtoValidator : AbstractValidator<OrganizationUnitDto>
    {
        public OrganizationUnitDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Organization Unit Name is required.")
                .Length(2, 64)
                .WithMessage("Name length must be between 2 and 64 characters.");
            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Organization Unit Type must be selected.");
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description may contain up to 500 characters.");
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .MaximumLength(500)
                .WithMessage("Address may contain up to 500 characters.");
            RuleFor(x => x.CountryId)
                .NotEmpty()
                .WithMessage("Country must be selected.");
            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.")
                .MaximumLength(500)
                .WithMessage("City may contain up to 500 characters.");
            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .WithMessage("Postal Code is required.")
                .MaximumLength(500)
                .WithMessage("Postal Code may contain up to 500 characters.");
            RuleFor(x => x.PhoneNumber)
                .Must((o, Phone) => ValidationHelper.IsValidPhone(Phone))
                .WithMessage("Phone number must be valid for your selected country.");
            RuleFor(x => x.VATNumber)
                .MaximumLength(100)
                .WithMessage("VAT may contain up to 100 characters.");

        }
    }
}
