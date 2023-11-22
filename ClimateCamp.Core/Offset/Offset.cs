using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    public class Offset : FullAuditedEntity<Guid>, IPassivable
    {

        public Offset(string title, string titleDescription, string detailsTitle, string description, string image, string price, string maximumVolume, DateTime creationDate, bool isActive)
        {
            this.Title = title;
            this.TitleDescription = titleDescription;
            this.DetailsTitle = detailsTitle;
            this.Description = description;
            this.Image = image;
            this.Price = price;
            this.MaximumVolume = maximumVolume;
            this.CreationDate = creationDate;
            this.IsActive = isActive;
        }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public string DetailsTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public string MaximumVolume { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public string Link { get; set; }
        public Guid? OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }

    }
}
