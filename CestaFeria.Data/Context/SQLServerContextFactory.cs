using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CestaFeria.Data.Context
{
    public class SQLServerContextFactory : IDesignTimeDbContextFactory<ApsContext>
    {
        // private readonly IConfiguration _configuration;

        // public ApsContextFactory(IConfiguration configuration)
        // {
        //     _configuration = configuration;
        // }
        // public ApsContext CreateDbContext(string[] args)
        // {

        //     var connectionString = _configuration.GetConnectionString("MySQLConnection");

        //     var optionsBuilder = new DbContextOptionsBuilder<ApsContext>();
        //     optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        //     return new ApsContext(optionsBuilder.Options);
        // }
        public ApsContext CreateDbContext(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var dir = Directory.GetParent(AppContext.BaseDirectory);

            var depth = 0;
            do
            {
                dir = dir?.Parent;
            } while (++depth < 5 && dir?.Name != "bin");

            dir = dir?.Parent;

            var basePath = dir?.FullName;
            var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            //var connectionString = config.GetConnectionString("MySQLConnection");

            var connectionString = "Server=localhost; Port=3306; Database=cestafeira; User=root; Password=123456;";

            var optionsBuilder = new DbContextOptionsBuilder<ApsContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            return new ApsContext(optionsBuilder.Options);
        }
    }
}