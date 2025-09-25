using MediatR;
using Microsoft.AspNetCore.Mvc;
using RulesEngine.Application.Invoices.UpdateInvoice;
using RulesEngine.Persistence.Abstractions;
using RulesEngineAPI.Contracts.Mundial;
using RulesEngineAPI.Extensions;

namespace RulesEngineAPI.Controllers
{
    /// <summary>
    /// Controlador encargado de procesar reglas asociadas a facturas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Procesamiento de Reglas")]
    public class ProcessRuleController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly IHttpContextAccessor _httpContext;

        /// <summary>
        /// Constructor del controlador de motor reglas .
        /// </summary>
        public ProcessRuleController(ISender sender, ICurrentTenantService currentTenantService, IHttpContextAccessor httpContext)
        {
            _sender = sender;
            _currentTenantService = currentTenantService;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Procesa una factura y aplica las reglas configuradas según el tenant.
        /// </summary>
        /// <param name="request">Datos de la factura a validar.</param>
        /// <returns>Resultado del procesamiento de la factura.</returns>
        /// <response code="200">motor de reglas ejecutado  correctamente.</response>
        /// <response code="400">Error de validación en la solicitud.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IResult> Post([FromBody] InvoiceToCheckRequest request)
        {
            // Construcción del comando a partir de la solicitud
            var command = new UpdateInvoiceCommand
            {
                Stage = request.Stage,
                RadNumber = request.RadNumber,
                ModuleName = request.ModuleName,
                TenantName = _httpContext.HttpContext?.Request.Headers["tenant"].ToString(),
                TenantId = _currentTenantService.TenantId
            };

            // Envío del comando al Mediator
            var result = await _sender.Send(command);

            // Manejo de respuesta funcional con patrón Result
            return result.Match(
                success => Results.Ok(success),
                error => result.ToProblem()
            );
        }
    }
}
