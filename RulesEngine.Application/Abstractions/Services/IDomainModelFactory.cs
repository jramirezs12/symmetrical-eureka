namespace RulesEngine.Application.Abstractions.Services
{
    public interface IDomainModelFactory
    {
        object Create(string tenant, string radNumber);
    }
}
