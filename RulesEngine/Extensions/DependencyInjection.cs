using RulesEngineAPI.Infrastructure;

namespace RulesEngineAPI.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAPI(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
            services.AddTransient<GlobalExceptionHandlerMiddleware>();
            return services;
        }
    }
}
