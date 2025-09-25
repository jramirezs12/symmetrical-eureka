using Microsoft.Extensions.DependencyInjection;
using RulesEngine.Application.Abstractions.Behaviors;
using RulesEngine.Application.Clients.Solidaria.Automapper;

namespace RulesEngine.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            });

            services.AddMemoryCache();
            services.AddAutoMapper(typeof(SolidariaProfile));
            //services.AddScoped<QueryExecutorPerPhase>();

            return services;
        }
    }
}