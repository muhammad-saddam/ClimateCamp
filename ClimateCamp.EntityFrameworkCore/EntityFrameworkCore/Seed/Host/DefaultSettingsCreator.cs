using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly CommonDbContext _context;

        public DefaultSettingsCreator(CommonDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            int? tenantId = null;

            if (ClimateCampConsts.MultiTenancyEnabled == false)
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }

            // Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "stijn@climatecamp.io", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "climatecamp.io mailer", tenantId);

            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", tenantId);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}
