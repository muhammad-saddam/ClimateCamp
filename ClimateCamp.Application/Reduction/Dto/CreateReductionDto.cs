﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(ClimateCamp.Core.Reduction))]
    public class CreateReductionDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public string DetailsTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool isFavourite { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public string Link { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
