using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ClimateCamp.Lookup;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    public class OrganizationUnit : FullAuditedEntity<Guid>, IPassivable
    {
        public enum OrganizationUnitType
        {
            Group, Unit, CostCenter, Division, Department, Team, Site
        }

        public OrganizationUnitType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string VATNumber { get; set; }
        public double MonthlyRevenue { get; set; }
        public int? CountryId { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? ParentOrganizationUnitId { get; set; }
        public virtual OrganizationUnit ParentOrganizationUnit { get; set; }

        public virtual ICollection<OrganizationUnit> Children { get; set; }
        public virtual Country Country { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }

        public Point Location { get; set; }
    }
}
