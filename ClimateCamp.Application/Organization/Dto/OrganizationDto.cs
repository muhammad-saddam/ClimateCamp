using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Common.Validation;
using ClimateCamp.Core;
using ClimateCamp.Core.CarbonCompute.Enum;
using ClimateCamp.Core.Editions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(Organization))]
    public class OrganizationDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string VATNumber { get; set; }
        public Guid BillingPreferenceId { get; set; }
        public int? CountryId { get; set; }
        public DateTime? AnnualReportingPeriod { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
        public IFormFile File { get; set; }
        public string PicturePath { get; set; }
        public string PictureName { get; set; }
        public int BaseLineYear { get; set; }
        public decimal BaseLineEmission { get; set; }
        public decimal Revenue { get; set; }
        public decimal Target { get; set; }
        public int ReportingFrequencyId { get; set; }
        public int? TotalEmployees { get; set; }
        public int? EditionId { get; set; }
        /// <summary>
        /// TODO: THe CustomEdition is an Entity type used in a DTO
        /// </summary>
        public virtual CustomEdition Edition { get; set; }
        public long? HubSpotId { get; set; }
        public long? ProductionQuantity { get; set; }
        public int? ProductionQuantityUnit { get; set; }
        public OrganizationStatus Status { get; set; }
    }

    public class OrganizationDtoValidator : AbstractValidator<OrganizationDto>
    {

        public OrganizationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Organization Name is required.")
                .Length(2, 64)
                .WithMessage("Name length must be between 2 and 64 characters.");
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .MaximumLength(500)
                .WithMessage("Address may contain up to 500 characters.");
            //RuleFor(x => x.PhoneNumber)
            //    .Must((o, Phone) => ValidationHelper.IsValidPhone(Phone))
            //    .WithMessage("Phone number must be valid for your selected country.");
            RuleFor(x => x.VATNumber)
                .MaximumLength(100)
                .WithMessage("VAT may contain up to 100 characters.");
            RuleFor(x => x.BillingPreferenceId)
                .NotEmpty()
                .WithMessage("Billing Preference must be selected.");
            RuleFor(x => x.CountryId)
                .NotEmpty()
                .WithMessage("Country must be selected.");
            RuleFor(x => x.TotalEmployees)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Number of employees must be greater than or equal to 0.");
            RuleFor(x => x.ReportingFrequencyId)
                .NotEmpty()
                .WithMessage("Reporting Frequencey must be selected."); ;

        }
    }

}
