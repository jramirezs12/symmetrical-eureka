using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.IpsRules
{
    public class AmbulanceLicensePlateRule29 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.AmbulanceControl != null && !string.IsNullOrEmpty(x.LicensePlateAmbulance) && !x.AmbulanceControl.Any(c => c.LicensePlate == x.LicensePlateAmbulance));

            Then()
                .Do(ctx => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de IPS",
                Module = "Reclamaciones",
                Description = "La placa de ambulancia del radicado no existe en la matriz REPS",
                Message = "Movil no se se encuentra habilitada en el REPS",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}