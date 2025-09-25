using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Application.Actions;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Mundial.Rules.RulesRepository.Fase_01.DuplicatedEventRules
{
    public class SameSoatAndVictimIdDifferentEventDateRule_33 : Rule, ITrackableRule
    {
        public Action OnMatch { get; set; } = () => { };
        public override void Define()
        {
            InvoiceToCheck? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Any(d => d.VictimId == x.VictimId && d.SoatNumber == x.SoatNumber
                                                                                            && !Date.IsNullable(x.EventDate)
                                                                                            && Date.Different(x.EventDate, d.EventDate)));

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()))
                .Do(ctx => OnMatch());
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Regla duplicidad de siniestro",
                AlertDescription = "El número de documento de identidad de la víctima y el número de póliza SOAT en la tabla origen es igual al número de documento de identidad de la víctima y el número de póliza SOAT en la tabla de consulta y la fecha de ocurrencia del evento en la tabla de origen es diferente a la fecha de ocurrencia del evento en la tabla de consulta",
                AlertMessage = "La víctima ya cuenta con un siniestro con otra fecha de accidente"
            };

            return alert;
        }
    }
}
