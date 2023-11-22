using Abp.Configuration;
using System.Collections.Generic;

namespace ClimateCamp.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, clientVisibilityProvider: new VisibleSettingClientVisibilityProvider()),
                //new SettingDefinition(
                //        "BlobStorageConnectionString",
                //        "DefaultEndpointsProtocol=https;AccountName=stclimatecampdeveu01;AccountKey=oeV+15wXN1vy/4Ex2xlm45XPwbdcjbvS9NYY8ajeV8KoTbSCVOV/82Q+ZMwBDl5e43oF0huYjAqnp0qeaGU4CA==;EndpointSuffix=core.windows.net"
                //        ),
                new SettingDefinition(
                        "OrganizationBlobContainerName",
                        "tenants"
                        ),
                //new SettingDefinition(
                //        "ClientUrl",
                //        "https://app-climatecamp-client-stg-001.azurewebsites.net/"
                //        ),
                new SettingDefinition(
                        "SetPasswordPageUrl",
                        "account/verify"
                        ),
                new SettingDefinition(
                        "ResetPasswordPageUrl",
                        "account/reset"
                        ),
                //new SettingDefinition(
                //        "EmailSenderFunctionUrl",
                //        "https://func-emailsender-stg-001.azurewebsites.net/api/SendEmail?code=MW1fXlTY0aipJWLjHIOr3bd84AMbjF8Rm13l3Ba9cNxAznJQiRsVhA=="
                //        ),
                new SettingDefinition(
                        "UserDefaultPassword",
                        "123@abc"
                        ),
                new SettingDefinition(
                        "SelfRegistrationPageUrl",
                        "account/register"
                        ),
                //new SettingDefinition(
                //        "FromEmail",
                //        "stijn@climatecamp.io"
                //        ),
                 //new SettingDefinition(
                 //       "DistanceActivityCalculationFunction",
                 //       "https://func-carboncompute-stg-001.azurewebsites.net/api/DistanceActivityCalculation?code=SmruLpylgZJ5cQlf6UN/6pEuLVYmz76dKjnTcPtBPjNlYZj3HMTfZA=="
                 //       ),
            };
        }
    }
}
