using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.LicensePlateAmbulance
{
    public class NitAndLicensePlateAmbulanceParametersStageIIRule16 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.AmbulanceControl!.Any(c => c.LicensePlate == x.LicensePlateAmbulance && x.IpsNit == c.NitIps));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "SendToQuality",
                AlertNameAction = "Enviar a Calidad",
                AlertType = "Regla por placa de ambulancia",
                AlertDescription = "Valida por nit y placa de ambulancia si existe coincidencia dentro del archivo matriz",
                AlertMessage = "Reclamación con habilitación de placa ambulancia"
            };

            return alert;
        }
    }
}
