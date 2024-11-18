using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeria.Data.Context;
using CestaFeria.Data.DataModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CestaFeira.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApsContext>(options =>
                options.UseMySql(
                configuration.GetConnectionString("MySQLConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("MySQLConnection")),
                b => b.MigrationsAssembly(typeof(ApsContext).Assembly.FullName)));

            services.AddScoped<IDataModule, DataModule<ApsContext>>();
            services.AddScoped<IDataModuleDBAps, DataModuleDBAps>();

            return services;
        }
    }
}