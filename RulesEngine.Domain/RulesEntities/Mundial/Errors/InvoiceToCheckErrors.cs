using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Domain.RulesEntities.Mundial.Errors
{
    public static class InvoiceToCheckErrors
    {
        public static Error ValidationErros { get; } = Error.Validation(
           description: "Se han generado varios errores de validación.");

        public static Error RadNumberEmpty { get; } = Error.Validation(
           description: "El número de radicado es requerido.");

        public static Error RadNumberLength { get; } = Error.Validation(
           description: "El número de radicado es de máximo 20 caracteres.");

        public static Error IpsNitEmpty { get; } = Error.Validation(
           description: "El Nit IPS es requerido.");

        public static Error IpsNitLength { get; } = Error.Validation(
           description: "El Nit IPS es de máximo 20 caracteres.");

        public static Error IpsNitNotNumber { get; } = Error.Validation(
           description: "El Nit IPS debe ser numérico.");

        public static Error SoatNumberLength { get; } = Error.Validation(
           description: "El número de poliza SOAT es de máximo 20 caracteres.");

        public static Error LicensePlateLength { get; } = Error.Validation(
           description: "La placa es de máximo 10 caracteres.");

        public static Error VictimIdLength { get; } = Error.Validation(
           description: "El documento de identidad es de máximo 16 caracteres.");

        public static Error DocumentTypeFormat { get; } = Error.Validation(
           description: "El tipo de documento debe ser: CC, CE, CN, PA, TI, RC, AS, MS, CD, SC, PE, PT, DE");

        public static Error EventDateFormat { get; } = Error.Validation(
           description: "La fecha del evento se encuentra en un fomato inválido");

        public static Error DeathDateFormat { get; } = Error.Validation(
           description: "La fecha de muerte se encuentra en un fomato inválido");

        public static Error ClaimDateFormat { get; } = Error.Validation(
           description: "La fecha reclamación se encuentra en un fomato inválido");

        public static Error InvoiceDateEmpty { get; } = Error.Validation(
           description: "La fecha de facturación es requerida");

        public static Error InvoiceDateFormat { get; } = Error.Validation(
           description: "La fecha de facturación se encuentra en un fomato inválido");

        public static Error IncomeDateFormat { get; } = Error.Validation(
           description: "La fecha de ingreso se encuentra en un fomato inválido");

        public static Error EgressDateFormat { get; } = Error.Validation(
           description: "La fecha de egreso se encuentra en un fomato inválido");

        public static Error InvoiceNumberF1Empty { get; } = Error.Validation(
           description: "El número de factura Furips1 es requerido");

        public static Error InvoiceNumberF1Length { get; } = Error.Validation(
           description: "El numero de factura Furips1 debe ser de máximo 20 caracteres.");

        public static Error InvoiceNumberF2Empty { get; } = Error.Validation(
           description: "El número de factura Furips2 es requerido");

        public static Error InvoiceNumberF2Length { get; } = Error.Validation(
           description: "El numero de factura Furips2 debe ser de máximo 20 caracteres.");

        public static Error PrimaryTransportationDateFormat { get; } = Error.Validation(
           description: "La fecha de transporte primario se encuentra en un fomato inválido");

        public static Error InvoiceMAOSDateFormat { get; } = Error.Validation(
           description: "La fecha factura proveedor MAOS se encuentra en un fomato inválido");

        public static Error InvestigationResponseDateFormat { get; } = Error.Validation(
           description: "La fecha de resultado de la investigación se encuentra en un fomato inválido");

        public static Error IpsNitF2Empty { get; } = Error.Validation(
           description: "El Nit IPS Furips2 es requerido");

        public static Error IpsNitF2NotNumber { get; } = Error.Validation(
            description: "El Nit IPS Furips2 debe ser numérico.");

        public static Error IpsNitF2Length { get; } = Error.Validation(
           description: "El Nit IPS Furips2 ser de máximo 20 caracteres.");

        public static Error InvoiceValueFormat { get; } = Error.Validation(
           description: "El valor de la factura se encuentra en un fomato inválido");

        public static Error InvoiceValueEmpty { get; } = Error.Validation(
          description: "El valor de la factura es requerido");

        public static Error InvoiceValueLength { get; } = Error.Validation(
          description: "El valor de la factura debe ser de máximo 20 caracteres.");

        public static Error BilledMedicalExpensesFormat { get; } = Error.Validation(
            description: "El total facturado gastos médicos se encuentra en un fomato inválido");

        public static Error BilledMedicalExpensesLength { get; } = Error.Validation(
          description: "El total facturado gastos médicos debe ser de máximo 20 caracteres.");

        public static Error BilledTransportationFormat { get; } = Error.Validation(
            description: "El Total facturado transporte se encuentra en un fomato inválido");

        public static Error BilledTransportationLength { get; } = Error.Validation(
          description: "El Total facturado transporte debe ser de máximo 20 caracteres.");

        public static Error IpsNitRipsNotNumber { get; } = Error.Validation(
            description: "El Nit IPS Rips debe ser numérico.");

        public static Error IpsNitRipsLength { get; } = Error.Validation(
         description: "El Nit IPS Rips debe ser de máximo 20 caracteres.");

        public static Error InvoiceNumberRipsLength { get; } = Error.Validation(
         description: "El número de factura Rips debe ser de máximo 20 caracteres.");

        public static Error IpsNitFuripsNotNumber { get; } = Error.Validation(
            description: "El Nit IPS Furips de Invoice Rips debe ser numérico.");

        public static Error IpsNitFuripsLength { get; } = Error.Validation(
         description: "El Nit IPS Furips de Invoice Rips debe ser de máximo 20 caracteres.");

        public static Error InvoiceNumberFuripsLength { get; } = Error.Validation(
         description: "El número de factura Furips de Invoice Rips debe ser de máximo 20 caracteres.");
    }
}
