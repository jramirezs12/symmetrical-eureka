using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Application.Actions;
using RulesEngine.Application.Actions.Common;
using RulesEngine.Application.Builders;
using RulesEngine.Application.Clients.Mundial.Invoices.Helper;
using RulesEngine.Application.DataSources;
using RulesEngine.Application.Mundial.Invoices.Helper;
using RulesEngine.Infrastructure.Actions;
using RulesEngine.Infrastructure.Builders;
using RulesEngine.Infrastructure.DataSources;
using RulesEngine.Infrastructure.Factories;
using RulesEngine.Infrastructure.Rules;
using RulesEngine.Infrastructure.Services;
using RulesEngine.Persistence;
using RulesEngine.Persistence.Abstractions;

namespace RulesEngine.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IShardingConnections, ShardingConnections>();
            services.AddScoped<ICurrentTenantService, CurrentTenantService>();
            services.AddScoped<IUtilityService, UtilityService>();

            services.AddScoped<IDomainModelProvider, RadDomainModelProvider>();
            services.AddScoped<IExternalDataLoader, ExternalDataLoaderMundial>();
            services.AddScoped<IExternalDataLoader, ExternalDataLoaderSolidaria>();
            services.AddScoped<IActionServiceFactory, ActionServiceFactory>();

            services.AddScoped<IDomainModelFactory, TenantDomainModelRegistry>();
            services. Scan(scan =>
            {
                scan.FromAssemblyOf<ITenantDomainModelBuilder>()
                    .AddClasses(c => c.AssignableTo<ITenantDomainModelBuilder>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
            services.AddScoped<IActionService, ActionServiceMundial>();
            services.AddScoped<IRuleSessionBuilder,RuleSessionBuilder>();
            services.AddScoped<IQueryExecutorPerPhase, QueryExecutorPerPhaseMundial>();
            services.AddScoped<IQueryExecutorPerPhaseSolidaria, QueryExecutorPerPhaseSolidaria>();
            services.AddScoped<IInvoiceToCheckBuilder, InvoiceToCheckBuilderMundial>();
            services.AddScoped<IInvoiceToCheckBuilder, InvoiceToCheckBuilderSolidaria>();
            services.AddScoped<IInvoiceToCheckBuilderFactory, InvoiceToCheckBuilderFactory>();
            services.AddScoped<IQueryExecutorPerPhaseFactory, QueryExecutorPerPhaseFactory>();
            services.AddPersistence(configuration);
            return services;
        }
    }
}
