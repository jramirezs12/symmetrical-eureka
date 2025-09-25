using Serilog.Context;

namespace RulesEngineAPI.Infrastructure
{
    /// <summary>
    /// Middleware que agrega propiedades de contexto a los logs de Serilog.
    /// </summary>
    
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor del middleware de contexto de logs.
        /// </summary>
        /// <param name="next">Delegado al siguiente middleware en la tubería.</param>
        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Método que se ejecuta en cada request entrante y agrega propiedades al contexto de logs.
        /// </summary>
        /// <param name="context">El contexto HTTP actual.</param>
        public Task InvokeAsync(HttpContext context)
        {
            // Se añade el CorrelationId y los headers al contexto de logs de Serilog
            using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
            using (LogContext.PushProperty("Cliente", context.Request.Headers.ToString()))
            {
                return _next(context);
            }
        }
    }
}
