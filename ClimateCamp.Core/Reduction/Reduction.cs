using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    public class Reduction : FullAuditedEntity<Guid>, IPassivable
    {

        public Reduction(string title, string titleDescription, string detailsTitle, string description, string image,
             DateTime creationDate, bool isActive)
        {
            this.Title = title;
            this.TitleDescription = titleDescription;
            this.DetailsTitle = detailsTitle;
            this.Description = description;
            this.Image = image;
            this.CreationDate = creationDate;
            this.IsActive = isActive;
        }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public string DetailsTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool isFavourite { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public Guid? OrganizationId { get; set; }
        public string Link { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }

    }
}
