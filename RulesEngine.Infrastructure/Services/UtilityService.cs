using Microsoft.Extensions.Caching.Memory;
using RulesEngine.Application.Abstractions.Services;

namespace RulesEngine.Infrastructure.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IMemoryCache _cache;

        public UtilityService(IMemoryCache cache)
        {
            _cache = cache;
        }


        /// <summary>
        ///  Método para agregar u obtener datos en cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"> llave por la cual se v a consultar la información </param>
        /// <param name="factory">función asíncrona que se ejecuta solo si el valor no está en cache</param>
        /// <returns></returns>
        public async Task<T> GetOrSetDataCache<T>(string key, Func<Task<T>> factory, int hoursToCache)
        {
            if (!_cache.TryGetValue(key, out T value))
            {
                value = await factory();
                _cache.Set(key, value, TimeSpan.FromHours(hoursToCache));
            }

            return value;
        }
    }
}
