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
               .Match(() => invoiceToCheck!, x => x.AmbulanceControl != null && !string.IsNullOrEmpty(x.LicensePlateAmbulance) && !x.AmbulanceControl.Any(c=>c.LicensePlate == x.LicensePlateAmbulance));
                                                  
            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla de IPS",
                AlertDescription = "Si la placa de ambulancia del radicado que se está validando no existe en la tabla de consulta",
                AlertMessage = "Movil no se se encuentra habilitada en el REPS"
            };
            return alert;
        }
    }
}
