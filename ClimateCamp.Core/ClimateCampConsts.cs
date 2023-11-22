using ClimateCamp.Common.Debugging;
using System;

namespace ClimateCamp.Core
{
    public static class ClimateCampConsts
    {
        public const string LocalizationSourceName = "ClimateCamp";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;

        /// <summary>
        /// Guid of the default emissions factors library (UK - BEIS)
        /// </summary>
        public static readonly Guid DefaultEmissionsFactorsLibraryId = Guid.Parse("A7801AD7-5312-4432-AB5C-C4472F0CDF1D");

        public const string AdminUserName = "admin@climatecamp.io";

        public const string UserAdminUserName = "user-admin@climatecamp.io";

        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5"
                : "d2aae3a47e2644caafa2fca5851499c2";
    }
}
