namespace RulesEngine.Infrastructure.Services;

public interface ITenantDomainModelBuilder
{
    string Tenant { get; }
    object Build(string radNumber);
}
