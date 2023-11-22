
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.ObjectModel;

namespace ClimateCamp.PowerBI
{
    public class PowerBIAuthentication : IPowerBIAuthentication
    {
        private readonly IPublicClientApplication _publicClientApplication;
        private readonly IConfiguration _config;

        public PowerBIAuthentication(IConfiguration config)
        {
            _config = config;

            _publicClientApplication = PublicClientApplicationBuilder
               .Create(_config.GetValue<string>("App:PowerBi:ClientId")) //PowerBIConstants.ClientId
               .WithTenantId(_config.GetValue<string>("App:PowerBi:TenantId")) //PowerBIConstants.TenantId
               .Build();

        }

        public dynamic GetTokenAsync()
        {
            AuthenticationResult result = null;
            try
            {
                try
                {
                    var tenantSpecificUrl = _config.GetValue<string>("App:PowerBi:AuthorityUrl").Replace("organizations", _config.GetValue<string>("App:PowerBi:TenantId"));  //PowerBIConstants.AuthorityUrl.Replace("organizations", PowerBIConstants.TenantId);
                    IConfidentialClientApplication clientApp = ConfidentialClientApplicationBuilder
                                                                                    .Create(_config.GetValue<string>("App:PowerBi:ClientId")) //PowerBIConstants.ClientId
                                                                                    .WithClientSecret(_config.GetValue<string>("App:PowerBi:ClientSecret")) //PowerBIConstants.ClientSecret
                                                                                    .WithAuthority(tenantSpecificUrl)
                                                                                    .Build();
                    result = clientApp.AcquireTokenForClient(PowerBIConstants.Scope).ExecuteAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            #region Catch Exception
            catch (MsalUiRequiredException)
            {
            }
            catch (MsalServiceException exception) when (exception.ErrorCode == "invalid_request")
            {

            }
            catch (MsalServiceException exception) when (exception.ErrorCode == "unauthorized_client")
            {

            }
            catch (MsalServiceException exception) when (exception.ErrorCode == "invalid_client")
            {
            }
            #endregion
            return result?.AccessToken;
        }
    }

    public static class PowerBIConstants
    {
        public static string AuthorityUrl = "https://login.microsoftonline.com/organizations/";
        public static string ClientId = "fe16572a-61ae-4105-9811-fe7c20ec3bc2";
        public static string TenantId = "99cbe2e0-4d82-435c-bb26-2b767d40408b";
        public static readonly ReadOnlyCollection<string> Scope = new ReadOnlyCollection<string>(new string[] {
    "https://analysis.windows.net/powerbi/api/.default"
  }
);
        public static string ClientSecret = "ACO7Q~EL3VmytcC1hMTrY0E1FaNCSDKR706Zm";
    }
}
