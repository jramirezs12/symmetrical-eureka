using RulesEngine.Persistence.Abstractions;

namespace RulesEngine.Infrastructure.Services
{
    public class CurrentTenantService(IShardingConnections conectionService) : ICurrentTenantService
    {
        private readonly IShardingConnections _conectionService = conectionService;

        public string TenantId { get; set; } = string.Empty;

        public string BasicTenantId { get; set; } = string.Empty;

        public async Task<bool> SetTenant(string tenant)
        {
            await Task.CompletedTask;
            // set tennant

            var DatabaseNames = _conectionService.GetNamedConnectionString(tenant);
            DatabaseNames.TryGetValue("GiproDbName", out string GiproDatabase);
            DatabaseNames.TryGetValue("BasicDbName", out string BasicDatabase);

            if (GiproDatabase is null ||
                BasicDatabase is null)
            {
                throw new ArgumentException("No se encontro tennant");
            }

            TenantId = GiproDatabase;
            BasicTenantId = BasicDatabase;

            return true;
        }
    }
}
