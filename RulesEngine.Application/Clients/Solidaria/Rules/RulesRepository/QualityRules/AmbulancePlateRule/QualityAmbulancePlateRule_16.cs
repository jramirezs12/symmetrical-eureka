using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.AmbulancePlate
{
    /// <summary>
    /// Assigns to Quality if ambulance plate is registered in parameter table
    /// </summary>
    public class QualityAmbulancePlateRule_16 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.AmbulanceControl!.Any(c => c.LicensePlate == x.LicensePlateAmbulance && x.IpsNit == c.NitIps));

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Enviar a Calidad",
                Type = "Regla por placa de ambulancia",
                Module = "Reclamaciones",
                Description = "Valida por nit y placa de ambulancia si existe coincidencia dentro del archivo matriz",
                Message = "Reclamación con habilitación de placa ambulancia",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}