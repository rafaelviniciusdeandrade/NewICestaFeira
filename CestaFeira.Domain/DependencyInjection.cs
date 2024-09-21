using CestaFeira.Domain.Interfaces.CryptographyService;
using CestaFeira.Domain.RequestBehavior;
using CestaFeira.Domain.Services.CryptographyService;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICryptographyService, CryptographyService>();


            //services.AddMediatR(typeof(ListaCompleteQuery).Assembly);



            return services;
        }

        public static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
        public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestBehaviorValidation<,>));

            return services;
        }
    }
}