using Abp;
using Abp.Dependency;
using Azure.Identity;
using ClimateCamp.Application;
using ClimateCamp.Common.Users.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Infrastructure.AzureADB2C
{
    /// <summary>
    /// GAzure AD Graph Client Service
    /// </summary>
    public class GraphClientService : AbpServiceBase, IGraphClientService, ITransientDependency
    {
        private readonly IConfiguration _config;
        private readonly GraphServiceClient _graphClient;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public GraphClientService(IConfiguration config)
        {
            _config = config;
            _graphClient = getGraphClient();
            _graphClient.HttpProvider.OverallTimeout = TimeSpan.FromHours(1);
        }

        /// <summary>
        /// Create Graph Service client
        /// </summary>
        /// <returns></returns>
        public GraphServiceClient getGraphClient()
        {
            Logger.Info("GraphClientService method : getGraphClient called !");
            try
            {
                var scopes = new[] { "https://graph.microsoft.com/.default" };
                var clientSecretCredential = new ClientSecretCredential(_config.GetValue<string>("App:AzureADB2C:TenantId"), _config.GetValue<string>("App:AzureADB2C:AppId"), _config.GetValue<string>("App:AzureADB2C:ClientSecret"));
                return new GraphServiceClient(clientSecretCredential, scopes);
            }

            catch (Exception ex)
            {
                Logger.Error("error occured at getGraphClient: " + ex.InnerException.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Create Azure AD B@C User
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<User> CreateUser(CreateUserDto input)
        {
            Logger.Info("GraphClientService method : CreateUser called !");
            // Declare the names of the custom attributes
            const string customAttributeName1 = "RoleId";
            const string customAttributeName2 = "OrganizationId";
            const string customAttributeName3 = "showFirstLoginExperience";

            // Get the complete name of the custom attribute (Azure AD extension)
            string roleIdAttribute = GetCompleteAttributeName(customAttributeName1);
            string organizationidAttribute = GetCompleteAttributeName(customAttributeName2);
            string showFirstLoginAttribute = GetCompleteAttributeName(customAttributeName3);

            Logger.Info($"Create a user with the custom attributes '{customAttributeName1}' (string) and '{customAttributeName2}' (boolean)");

            // Fill custom attributes
            IDictionary<string, object> extensionInstance = new Dictionary<string, object>();
            extensionInstance.Add(roleIdAttribute, input.RoleId);
            extensionInstance.Add(organizationidAttribute, input.OrganizationId);
            extensionInstance.Add(showFirstLoginAttribute, true);
            try
            {
                var result = await _graphClient.Users
               .Request()
               .AddAsync(new User
               {
                   GivenName = input.Name,
                   Surname = input.Surname,
                   DisplayName = input.Name + " " + input.Surname,
                   Identities = new List<ObjectIdentity>
                   {
                         new ObjectIdentity()
                         {
                             SignInType = "emailAddress",
                             Issuer = _config.GetValue<string>("App:AzureADB2C:TenantId"),
                             IssuerAssignedId = input.EmailAddress
                         }
                   },
                   PasswordProfile = new PasswordProfile()
                   {
                       Password = GenerateNewPassword(4, 8, 4)
                   },
                   PasswordPolicies = "DisablePasswordExpiration",
                   AdditionalData = extensionInstance
               });
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("error occured at getGraphClient: " + ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Update Azure AD B2C user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<User> UpdateUser(UserDto input)
        {
            // Declare the names of the custom attributes
            const string customAttributeName1 = "RoleId";
            const string customAttributeName2 = "OrganizationId";
            const string customAttributeName3 = "showFirstLoginExperience";

            // Get the complete name of the custom attribute (Azure AD extension)
            string roleIdAttribute = GetCompleteAttributeName(customAttributeName1);
            string organizationidAttribute = GetCompleteAttributeName(customAttributeName2);
            string showFirstLoginAttribute = GetCompleteAttributeName(customAttributeName3);

            // Fill custom attributes
            IDictionary<string, object> extensionInstance = new Dictionary<string, object>();
            extensionInstance.Add(roleIdAttribute, input.RoleId);
            extensionInstance.Add(organizationidAttribute, input.OrganizationId);
            extensionInstance.Add(showFirstLoginAttribute, input.IsFirstLoginExperience);
            try
            {
                var userId = await GetUserBySignInName(input.EmailAddress);
                var result = await _graphClient.Users[userId.Id]
                      .Request()
                      .UpdateAsync(new User
                      {
                          GivenName = input.Name,
                          Surname = input.Surname,
                          DisplayName = input.Name + " " + input.Surname,
                          Identities = new List<ObjectIdentity>
                                     {
                                         new ObjectIdentity()
                                         {
                                             SignInType = "emailAddress",
                                             Issuer = _config.GetValue<string>("App:AzureADB2C:TenantId"),
                                             IssuerAssignedId = input.EmailAddress
                                         }
                                     },
                          PasswordProfile = new PasswordProfile()
                          {
                              Password = input.Password
                          },
                          PasswordPolicies = "DisablePasswordExpiration",
                          AdditionalData = extensionInstance
                      });


                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("error occured at getGraphClient: " + ex.Message.ToString());
                return null;
            }
        }


        /// <summary>
        /// Delete User By email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserById(string email)
        {

            var user = await GetUserBySignInName(email);
            try
            {
                //// Delete user by object ID
                await _graphClient.Users[user.Id]
                   .Request()
                   .DeleteAsync();
                return true;


            }
            catch (Exception ex)
            {
                Logger.Error("error occured at getGraphClient: " + ex.Message.ToString());
                return false;
            }
        }
        /// <summary>
        /// Get User By email
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        public async Task<User> GetUserById(string userId)
        {
            try
            {
                // Get user by object ID
                var result = await _graphClient.Users[userId]
                    .Request()
                    .Select(e => new
                    {
                        e.DisplayName,
                        e.Id,
                        e.Identities
                    })
                    .GetAsync();
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("error occured at getGraphClient: " + ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// List All users
        /// </summary>
        /// <returns></returns>
        public async Task<IGraphServiceUsersCollectionPage> ListUsers()
        {
            try
            {
                // Get all users
                var users = await _graphClient.Users
                    .Request()
                    .Select(e => new
                    {
                        e.DisplayName,
                        e.Id,
                        e.Identities
                    })
                    .GetAsync();

                return users;

            }
            catch (Exception ex)
            {
                Logger.Error("error occured at getGraphClient: " + ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Get User by email 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUserBySignInName(string userId)
        {
            try
            {
                // Get user by sign-in name
                var result = await _graphClient.Users
                    .Request()
                    .Filter($"identities/any(c:c/issuerAssignedId eq '{userId}' and c/issuer eq '{_config.GetValue<string>("App:AzureADB2C:TenantId")}')")
                    .Select(e => new
                    {
                        e.DisplayName,
                        e.Id,
                        e.Identities
                    })
                    .GetAsync();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Error("error occured at getGraphClient: " + ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// Set User Password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> SetPasswordByUserId(string userId, string password)
        {
            var userAD = await GetUserBySignInName(userId);
            var user = new User
            {
                PasswordPolicies = "DisablePasswordExpiration,DisableStrongPassword",
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = false,
                    Password = password,
                }
            };
            try
            {
                // Update user by object ID
                var result = await _graphClient.Users[userAD.Id]
                    .Request()
                    .UpdateAsync(user);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("error occured at getGraphClient: " + ex.Message.ToString());
                return null;
            }
        }

        internal string GetCompleteAttributeName(string attributeName)
        {
            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new System.ArgumentException("Parameter cannot be null", nameof(attributeName));
            }

            return $"extension_{_config.GetValue<string>("App:AzureADB2C:B2cExtensionAppClientId").Replace("-", "")}_{attributeName}";
        }
        /// <summary>
        /// generate dummy password
        /// </summary>
        /// <param name="lowercase"></param>
        /// <param name="uppercase"></param>
        /// <param name="numerics"></param>
        /// <returns></returns>
        public static string GenerateNewPassword(int lowercase, int uppercase, int numerics)
        {
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";

            Random random = new Random();

            string generated = "!";
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);
        }
    }
    /// <summary>
    /// Global strings
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// 
        /// </summary>
        public static string aadInstance = "https://login.microsoftonline.com/";
        /// <summary>
        /// 
        /// </summary>
        public static string aadGraphResourceId = "https://graph.windows.net/";
        /// <summary>
        /// 
        /// </summary>
        public static string aadGraphEndpoint = "https://graph.windows.net/";
        /// <summary>
        /// 
        /// </summary>
        public static string aadGraphSuffix = "";
        /// <summary>
        /// 
        /// </summary>
        public static string aadGraphVersion = "api-version=1.6";
    }
}
