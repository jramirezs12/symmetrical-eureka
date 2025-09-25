using RulesEngineContracts.Mundial;

namespace RulesEngineAPI.Infrastructure
{
    /// <summary>
    /// Middleware global para el manejo centralizado de excepciones en la aplicación.
    /// </summary>
    /// <remarks>
    /// Este middleware:
    /// - Intercepta cualquier excepción no controlada en el pipeline.
    /// - Registra la excepción usando <see cref="ILogger"/>.
    /// - Retorna un objeto <see cref="InvoiceToCheckResponse"/> como respuesta en formato JSON.
    /// 
      /// </remarks>
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Constructor del middleware de manejo global de excepciones.
        /// </summary>
        /// <param name="logger">Instancia del logger para registrar errores.</param>
        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Método que se ejecuta en cada request entrante.
        /// </summary>
        /// <param name="context">El contexto HTTP actual.</param>
        /// <param name="next">El siguiente delegado en el pipeline de middleware.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Continúa con el siguiente middleware o controlador
                await next(context);
            }
            catch (Exception ex)
            {
                // Registrar el error con su stack trace y mensaje
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);

                // Por defecto usaríamos un 500 (Internal Server Error)
                var status = StatusCodes.Status500InternalServerError;
                var detail = ex.Message;
                int rulesExecuted = default;

                var res = new InvoiceToCheckResponse(
                    status,
                    detail,
                    rulesExecuted,
                    null
                );

                context.Response.StatusCode = StatusCodes.Status200OK;

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(res);
            }
        }
    }
}
