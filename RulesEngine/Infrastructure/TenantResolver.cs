using RulesEngine.Persistence.Abstractions;

namespace RulesEngineAPI.Infrastructure
{
    /// <summary>
    /// Middleware para resolver el Tenant actual en base al encabezado "tenant" de la solicitud.
    /// </summary>
    /// <remarks>
    /// Este middleware intercepta la petición HTTP y valida que el header "tenant" exista.
    /// - Si el header está presente, configura el tenant en el servicio <see cref="ICurrentTenantService"/>.
    /// - Si no existe, lanza una excepción y corta la ejecución.
    /// 
    /// Debe registrarse en el pipeline de ASP.NET Core mediante <c>app.UseMiddleware&lt;TenantResolver&gt;();</c>
    /// en el archivo Program.cs o Startup.cs.
    /// </remarks>
    public class TenantResolver
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="TenantResolver"/>.
        /// </summary>
        /// <param name="next">Delegado al siguiente middleware en la tubería.</param>
        public TenantResolver(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Método invocado en cada request entrante.
        /// </summary>
        /// <param name="context">El contexto HTTP actual.</param>
        /// <param name="currentTenantService">Servicio que administra el Tenant actual.</param>
        /// <exception cref="InvalidOperationException">Se lanza si el header "tenant" no está presente.</exception>
        public async Task InvokeAsync(HttpContext context, ICurrentTenantService currentTenantService)
        {
            // Intentamos obtener el header "tenant"
            context.Request.Headers.TryGetValue("tenant", out var tenantFromHeader);

            if (!string.IsNullOrEmpty(tenantFromHeader))
            {
                // Configura el tenant actual en el servicio
                await currentTenantService.SetTenant(tenantFromHeader!);
            }
            else
            {
                // Si no se encuentra el tenant, se interrumpe la petición
                throw new InvalidOperationException("Tenant not found in request headers.");
            }

            // Continua con el siguiente middleware en la tubería
            await _next(context);
        }
    }
}
