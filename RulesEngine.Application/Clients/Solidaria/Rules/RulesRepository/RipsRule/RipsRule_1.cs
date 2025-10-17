using NRules.Fluent.Dsl;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.RulesEntities.Solidaria.Entities;

namespace RulesEngine.Application.Clients.Solidaria.Rules.RulesRepository.RipsRule
{
    public class RipsRule_1 : Rule
    {
        public override void Define()
        {
            InvoiceToCheckSolidaria? invoiceToCheck = default;

            When()
                .Match(() => invoiceToCheck!, x => x.IpsNit != x.IpsNitRips
                                                    || x.InvoiceNumberRips != x.InvoiceNumberF1);

            Then()
                .Do(w => invoiceToCheck!.AlertSolidaria.Add(CreateAlert()));
        }

        private static AlertSolidaria CreateAlert()
        {
            return new AlertSolidaria
            {
                NameAction = "Alerta",
                Type = "Regla de RIPS",
                Module = "Reclamaciones",
                Description = "El NIT más número de factura de la tabla origen no coincide con el registrado en la tabla de consulta",
                Message = "Mostrar mensaje indicando que no fue aportado RIPS",
                Typification = string.Empty,
                HasPriority = false
            };
        }
    }
}