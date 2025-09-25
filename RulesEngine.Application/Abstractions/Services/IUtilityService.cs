namespace RulesEngine.Application.Abstractions.Services
{
    public interface IUtilityService
    {
        Task<T> GetOrSetDataCache<T>(string key, Func<Task<T>> factory, int hoursToCache);
    }
}
