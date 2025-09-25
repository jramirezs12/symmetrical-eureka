using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.IpsRules
{
    public class AmbulanceLicensePlateRule29 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
               .Match(() => invoiceToCheck!, x => x.AmbulanceControl != null && !string.IsNullOrEmpty(x.LicensePlateAmbulance) && !x.AmbulanceControl.Any(c=>c.LicensePlate == x.LicensePlateAmbulance));
                                                  
            Then()
                .Do(ctx => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
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
