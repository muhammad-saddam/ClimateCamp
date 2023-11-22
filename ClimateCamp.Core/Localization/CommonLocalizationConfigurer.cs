using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace ClimateCamp.Core.Localization
{
    public static class CommonLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(ClimateCampConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(CommonLocalizationConfigurer).GetAssembly(),
                        "ClimateCamp.Common.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
