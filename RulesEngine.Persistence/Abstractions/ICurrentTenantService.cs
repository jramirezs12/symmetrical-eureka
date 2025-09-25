namespace RulesEngine.Persistence.Abstractions
{
    public interface ICurrentTenantService
    {
        public string BasicTenantId { get; set; }
        public string TenantId { get; set; }
        public Task<bool> SetTenant(string tenant);
    }
}
