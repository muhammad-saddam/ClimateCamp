using ClimateCamp.Core.Editions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultEditionCreator
    {
        private readonly CommonDbContext _context;
        public static List<CustomEdition> InitialEditions => GetInitialEditions();
        public DefaultEditionCreator(CommonDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            foreach (var edition in InitialEditions)
            {
                AddEditionIfNoExist(edition);
            }

        }

        private void AddEditionIfNoExist(CustomEdition edition)
        {
            if (_context.CustomEditions.IgnoreQueryFilters().Any(t => t.Name == edition.Name))
            {
                return;
            }
            _context.Editions.Add(edition);
            _context.SaveChanges();

            /* Add desired features to the standard edition, if wanted... */
        }

        private static List<CustomEdition> GetInitialEditions()
        {
            return new List<CustomEdition>
            {
                new CustomEdition{ Name = EditionManager.DefaultEditionName, DisplayName = "Climatecamp Enterprise+ Edition",Image = "../../../assets/img/billings/billing-placeholder.png", PriceLabel ="Pricing On Request", IsContactSales = true},
                new CustomEdition{ Name = "Enterprise", DisplayName = "Climatecamp Enterprise Edition",Image = "../../../assets/img/billings/billing-placeholder.png", PriceLabel ="2500€/Month", IsContactSales = true},
                new CustomEdition{ Name = "Business", DisplayName = "Climatecamp Business Edition",Image = "../../../assets/img/billings/billing-placeholder.png", PriceLabel ="500€/Month", IsContactSales = true},
                new CustomEdition{ Name = "Expense Partner", DisplayName = "Climatecamp Expense Partner Edition",Image = "../../../assets/img/billings/billing-placeholder.png", PriceLabel ="Free", IsContactSales = false},
            };
        }
    }
}
