using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RulesEngine.Domain.BlobsStorage.Repositories;
using RulesEngine.Domain.ClaimsQueue.Repository;
using RulesEngine.Domain.Constants;
using RulesEngine.Domain.DisputeProcess.Repository;
using RulesEngine.Domain.ElectronicBillingRuleEngine.Repository;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.LegalProceedings.Repository;
using RulesEngine.Domain.Parameters.Repositories;
using RulesEngine.Domain.Provider.Repository;
using RulesEngine.Domain.Research.Repository;
using RulesEngine.Domain.RulesEngine.Repositories;
using RulesEngine.Persistence.Configurations.DataBaseConfiguration;
using RulesEngine.Persistence.Repositories;
using RulesEngine.Persistence.Settings;

namespace RulesEngine.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GiproConnectionSettings>(configuration.GetSection(GiproConnectionSettings.SectionName));
            services.Configure<BasicMongoConnectionSettings>(configuration.GetSection(BasicMongoConnectionSettings.SeccionName));

            services.AddScoped(typeof(GiproDbContext));
            services.AddScoped(typeof(BasicGiproDbContext));
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();
            services.AddScoped<IRuleEngineRepository, RuleEngineRepository>();
            services.AddScoped<IParameterRepository, ParameterRepository>();
            services.AddScoped<IConstantsRepository, ConstantsRepository>();
            services.AddScoped<IElectronicBillingRepository, ElectronicBillingRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<ILegalProcessesAndTransactionContractsRepository, LegalProcessesAndTransactionContractsRepository>();
            services.AddScoped<IResearchRepository, ResearchRepository>();
            services.AddScoped<IClaimsQueueRepository, ClaimsQueueRepository>();
            services.AddScoped<ITransactionContractsRepository, TranssactionContractsRepository>();
            services.AddScoped<ILegalProceedingsRepository, LegalProceedingsRepository>();
            return services;
        }
    }
}