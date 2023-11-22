using Abp.Authorization.Users;
using Abp.Extensions;
using ClimateCamp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Common.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "P@ssw0rd";
        public string Phone { get; set; }
        public Guid? OrganizationId { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public bool IsFirstLoginExperience { get; set; }
        [NotMapped]
        public bool IsSelfServiceUser { get; set; }
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                Name = "John",
                Surname = "Doe",
                UserName = ClimateCampConsts.UserAdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public static implicit operator string(User v)
        {
            throw new NotImplementedException();
        }
    }
}
