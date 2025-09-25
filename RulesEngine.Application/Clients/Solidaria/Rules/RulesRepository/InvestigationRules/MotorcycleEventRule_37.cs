using Autofac;
using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.InvestigationRules
{
    public class MotorcycleEventRule_37 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.DuplicatedInvoices.Where(d => !string.IsNullOrWhiteSpace(d.VehicleType) && Convert.ToInt32(d.VehicleType) == 10 && d.LicensePlate == x.LicensePlate
                                                                            &&
                                                                            (
                                                                                d.VictimId != x.VictimId
                                                                                || d.VictimId == x.VictimId && !Date.IsNullable(x.EventDate) && Date.Different(x.EventDate, d.EventDate)
                                                                            ))
                                                                        .Select(y => new { y.VictimId, y.EventDate }).Distinct().Count()
                                                                        > x.SamePlateForMotorcycleDifferentEventNumber);

            Then()
                .Do(w => invoiceToCheck!.Alerts.Add(CreateAlert()));
        }

        private static Alert CreateAlert()
        {
            var alert = new Alert
            {
                AlertAction = "Alert",
                AlertNameAction = "Alerta",
                AlertType = "Reglas de investigación",
                AlertDescription = "El tipo de vehículo es 10, y si la placa en la tabla de origen es igual a la placa en la tabla de consulta, debe construir la llave \"Siniestro\" en la tabla de origen y contar la cantidad de siniestros diferentes en la tabla consulta , si la cantidad de Siniestros es mayor a x",
                AlertMessage = "Evento múltiple"
            };

            return alert;
        }
    }
}
