using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_02.LicensePlateAmbulance
{
    public class NitAndLicensePlateAmbulanceParametersStageIIRule16 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck, x => x.AmbulanceControl!.Any(c => c.LicensePlate == x.LicensePlateAmbulance && x.IpsNit == c.NitIps));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
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
