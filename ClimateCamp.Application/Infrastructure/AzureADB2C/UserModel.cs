// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Graph;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClimateCamp.Infrastructure.AzureADB2C
{
    public class UserModel : User
    {
        [JsonPropertyName("password")]
        public string Password { get; set; }
        public string OrganizationId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsFirstLoginExperience { get; set; }
        public bool IsActive { get; set; }
        public void SetB2CProfile(string TenantName)
        {
            this.PasswordProfile = new PasswordProfile
            {
                ForceChangePasswordNextSignIn = false,
                Password = this.Password,
                ODataType = null
            };
            this.PasswordPolicies = "DisablePasswordExpiration,DisableStrongPassword";
            this.Password = null;
            this.ODataType = null;

            foreach (var item in this.Identities)
            {
                if (item.SignInType == "emailAddress" || item.SignInType == "userName")
                {
                    item.Issuer = TenantName;
                }
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}