using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultReductionCreator
    {


        private readonly CommonDbContext _context;
        public static List<Reduction> InitialReductions => GetInitialReductions();
        public DefaultReductionCreator(CommonDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateReductions();
        }

        private void CreateReductions()
        {
            foreach (var reduction in InitialReductions)
            {
                AddReductionIfNotExists(reduction);
            }
        }

        private void AddReductionIfNotExists(Reduction reduction)
        {
            if (_context.Reductions.IgnoreQueryFilters().Any(t => t.Title == reduction.Title))
            {
                return;
            }

            _context.Reductions.Add(reduction);
            _context.SaveChanges();
        }

        private static List<Reduction> GetInitialReductions()
        {
            return new List<Reduction>
            {
                new Reduction("Bike To work",
                "Motivate your colleagues to chose as much as possible their bike over their car.",
                "Start a campaign to let all your employees and colleagues use their bike to commute to their work. We have some best practice suggestions.",
              "<h3><strong>Bike Leasing</strong></h3><h3><strong>Bicycle allowance</strong></h3><h3><strong>Dressing rooms & showers</strong></h3>Sweaty bikers like to shower and change before work.Also a breakfast area or meal energizes after the work-outand helps to start a productive workday. <h3><strong>A safe bike park</strong></h3>It's important to park an expensive bike safely and dry. So the owner does not have to worry all day.",
              "../../assets/img/reduction/bike.png",
              DateTime.UtcNow,
              true),
                 new Reduction("EV car policy",
                "Start today with changing leasing cars to EV models.",
                "EV car policy",
                "",
                "../../assets/img/reduction/EV_car.png",
              DateTime.UtcNow,
              true),
                 new Reduction("Solar panels",
                "Every roof is a potential energy producer.",
                "Solar panels",
                "",
                "../../assets/img/reduction/solar_panels.png",
              DateTime.UtcNow,
              true),
                 new Reduction("Green energy contracts",
                "Make sure your energy provider is green.",
                "Green energy contracts",
                "",
                "../../assets/img/reduction/green_energy.png",
              DateTime.UtcNow,
              true),
                 new Reduction("LED office lighting",
                "Reducing energy consumers, saves money and carbon emissions.",
                "LED office lighting",
                "",
                "../../assets/img/reduction/light_bulb.png",
              DateTime.UtcNow,
              true),
                 new Reduction("office heating",
                "Is your building well isolated? Is your boiler a new efficient model?",
                "office heating",
                "",
                "../../assets/img/reduction/heating_f.png",
              DateTime.UtcNow,
              true)
            };
        }
    }
}