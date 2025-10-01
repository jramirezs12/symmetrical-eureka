using RulesEngine.Domain.Invoices.Entities;

namespace RulesEngine.Application.Mundial.Invoices.Helper
{
    // Solo métodos adicionales específicos de la capa aplicación
    internal static class SolidariaInvoiceDataLogicExtensions
    {
        // Ejemplo adicional (no duplicado):
        public static bool HasVictims(this SolidariaInvoiceData d)
            => d.Claim?.Victims != null && d.Claim.Victims.Count > 0;
    }
}