using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ClimateCamp.EntityFrameworkCore
{
    public static class CommonDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<CommonDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString, x => x.UseNetTopologySuite());
        }

        public static void Configure(DbContextOptionsBuilder<CommonDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection, x => x.UseNetTopologySuite());
        }
    }
}
