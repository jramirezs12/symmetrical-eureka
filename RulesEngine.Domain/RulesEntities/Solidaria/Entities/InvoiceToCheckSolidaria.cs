using RulesEngine.Application.DataSources;
using RulesEngine.Domain.Agregations.Entities;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.DisputeProcess.Entities;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Primitives;
using RulesEngine.Domain.Provider.Entities;
using RulesEngine.Domain.Research.Entities;
using RulesEngine.Domain.ValueObjects;
using System.Collections.Generic;

namespace RulesEngine.Domain.RulesEntities.Solidaria.Entities
{
    /// <summary>
    /// Contexto unificado de datos que serán evaluados por las reglas para el tenant Solidaria.
    /// Esta clase es un “snapshot” enriquecido: mezcla datos primarios de la factura (SolidariaInvoiceData)
    /// con datos externos (agregaciones, catálogos, blobs, cálculos) para no acoplar las reglas
    /// directamente al modelo de persistencia.
    ///
    /// NOTA SOBRE DIFERENCIAS VS FURIPS (Mundial):
    /// - Ya no existe Sections.*; ahora los datos se obtienen de:
    ///     * Claim (equivale a claimData / víctima / evento / vehículo)
    ///     * Claim.Victims[0] (víctima principal)
    ///     * Claim.Vehicle (vehículo involucrado / placa / soat)
    ///     * Claim.Event (evento / fecha / dirección / etc.)
    ///     * Claim.TotalGlossValues (totales de glosas)
    ///     * Claim.Victims[].Services[].Glosses (detalle de glosas)
    /// - Campos históricos como InvoiceNumberF1 / F2, IpsNitF2 se mapean al único
    ///   número de factura principal o se mantienen por compatibilidad (para no
    ///   romper reglas existentes que esperan esas propiedades).
    /// - InvoiceInformation (detalle de glosas FURIPS) no existe en Solidaria;
    ///   se deja nullable para compatibilidad si alguna regla la consulta,
    ///   pero se prefiere migrar a usar Claim.Victims[].Services[].Glosses.
    /// </summary>
    public partial class InvoiceToCheckSolidaria : InputSourcesEntitty, IInvoiceToCheckContext, IRuleMetadataAware
    {
        public string RadNumber { get; private set; }
        private readonly IExternalDataLoader _loader;

        public InvoiceToCheckSolidaria(string radNumber, IExternalDataLoader loader)
        {
            RadNumber = radNumber;
            _loader = loader;
        }

        public enum DocumentTypeEnum { CC = 1, DE, NIT, TI, PA, TSS, SEN, FDI, RC, AS, MS, TP, PE, PT, CN, SC, CD }

        /// <summary> NIT del prestador (Provider.IdNumber) </summary>
        public string IpsNit { get; set; } = string.Empty;

        /// <summary> Estado / módulo de proceso de la factura (BusinessInvoiceStatus u otro campo lógico) </summary>
        public string ModuleName { get; set; } = string.Empty;

        /// <summary> Número de póliza SOAT o SIRAS (Claim.Vehicle.Soat.Policy.Number | SIRASFilingNumber) </summary>
        public string SoatNumber { get; set; } = string.Empty;

        /// <summary> Placa del vehículo (Claim.Vehicle.PlateNumber) </summary>
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary> Identificación de la víctima principal (Claim.Victims[0].IdNumber) </summary>
        public string VictimId { get; set; } = string.Empty;

        /// <summary> Tipo documento víctima (Claim.Victims[0].IdType.Code) </summary>
        public string DocumentType { get; set; } = string.Empty;

        /// <summary> Fecha del evento (Claim.Event.Date) </summary>
        public Date EventDate { get; set; }

        /// <summary> Fecha fallecimiento (Claim.Victims[0].DeathInfo.DeathDate) </summary>
        public Date DeathDate { get; set; }

        /// <summary> Fecha de radicación / reclamación (FillingDate o InsuranceFillingDate) </summary>
        public Date ClaimDate { get; set; }

        /// <summary> Fecha de emisión de la factura (InvoiceEmissionDate) </summary>
        public Date InvoiceDate { get; set; }

        /// <summary> Fecha ingreso atención médica (Victim.MedicalAttention.IncomeDate) </summary>
        public Date IncomeDate { get; set; }

        /// <summary> Fecha egreso atención médica (Victim.MedicalAttention.EgressDate) </summary>
        public Date EgressDate { get; set; }

        /// <summary> Número de factura “F1” (compatibilidad; Solidaria usa uno principal) </summary>
        public string InvoiceNumberF1 { get; set; } = string.Empty;

        /// <summary> Número de factura “F2” (compatibilidad; reutiliza el mismo de Solidaria) </summary>
        public string InvoiceNumberF2 { get; set; } = string.Empty;

        /// <summary> Fecha de transporte primario (No análogo directo; se mantiene para futuras extensiones) </summary>
        public Date PrimaryTransportationDate { get; set; }

        /// <summary> Fecha factura proveedor MAOS (sin análogo directo; se deja para compatibilidad) </summary>
        public Date InvoiceMAOSDate { get; set; }

        /// <summary> Fecha de resultado investigación (max ResponseDate filtrado) </summary>
        public Date InvestigationResponseDate { get; set; }

        /// <summary> NIT proveedor en “segunda reclamación” (compatibilidad FURIPS) </summary>
        public string IpsNitF2 { get; set; } = string.Empty;

        /// <summary> Valor de la factura (InvoiceValue) </summary>
        public Currency InvoiceValue { get; set; } = Currency.Create("0");

        /// <summary> Total facturado gastos médicos (Victim.ProtectionsClaimed.MedicalSurgicalExpenses.TotalBilled) </summary>
        public Currency BilledMedicalExpenses { get; set; } = Currency.Create("0");

        /// <summary> Total facturado transporte (Victim.ProtectionsClaimed.VictimTransportAndMobilizationExpenses.TotalBilled) </summary>
        public Currency BilledTransportation { get; set; } = Currency.Create("0");

        /// <summary> (Compatibilidad) NIT RIPS si existiera proceso paralelo </summary>
        public string IpsNitRips { get; set; } = string.Empty;

        public string InvoiceNumberRips { get; set; } = string.Empty;

        public string IpsNitFurips { get; set; } = string.Empty;

        public string InvoiceNumberFurips { get; set; } = string.Empty;

        /// <summary> NIT factura electrónica (ElectronicBilling.NitIps) </summary>
        public string IpsNitFE { get; set; } = string.Empty;

        /// <summary> Número factura electrónica (ElectronicBilling.InvoiceNumber) </summary>
        public string InvoiceNumberFE { get; set; } = string.Empty;

        /// <summary> Parámetro de verificación telefónica (monto) </summary>
        public Currency InvoicePhoneVerificationValue { get; set; } = Currency.Create("0");

        /// <summary> Métricas de ocurrencias de placa vs fechas (si se calculan) </summary>
        public int SamePlateDifferentEventNumber { get; set; }

        public int SamePlateForMotorcycleDifferentEventNumber { get; set; }

        public int SameVictimIdDifferentEventNumber { get; set; }

        /// <summary> Valor total glosado (Claim.TotalGlossValues.TotalInvoiceObjectedValue) </summary>
        public Currency TotalGlossedValue { get; set; } = Currency.Create("0");

        /// <summary> Valor total autorizado / aprobado (Claim.TotalGlossValues.TotalInvoiceApprovedValue) </summary>
        public Currency TotalAuthorizedValue { get; set; } = Currency.Create("0");

        /// <summary> Tipo de amparo (ProtectionType.Value) </summary>
        public string HelpType { get; set; } = string.Empty;

        /// <summary> Valor parametrizado para comparar amparo </summary>
        public string HelpTypeToValidate { get; set; } = string.Empty;

        /// <summary> Usuario que hizo la reclamación (ClaimsQueue.UserAccount) </summary>
        public string UserClaim { get; set; } = string.Empty;

        /// <summary> Placa de ambulancia (Victim.RemissionInfo.Transport.PrimaryTransferAmbulancePlate) </summary>
        public string LicensePlateAmbulance { get; set; } = string.Empty;

        /// <summary> Número de factura general (InvoiceNumber) </summary>
        public string InvoiceNumber { get; set; } = string.Empty;

        /// <summary> Validación siniestro agregado (sinister query) </summary>
        public SinisterAggregation SinisterAggretation { get; set; }

        /// <summary> Procesos legales y contratos (agregación legal / dispute) </summary>
        public List<DisputeProcessEntity> ProcessAndContracts { get; set; } = new();

        /// <summary> Alertas base (SolidariaInvoiceData.Alerts) convertidas </summary>
        public List<AlertSolidaria> AlertSolidaria { get; set; } = [];

        /// <summary> Resultados de investigación (ResearchData) </summary>
        public ResearchEntity[] Research { get; set; } = [];

        /// <summary>
        /// Información legacy de glosas (solo para compatibilidad;
        /// en Solidaria usar Claim.Victims[].Services[].Glosses y Claim.TotalGlossValues)
        /// </summary>
        public InvoiceInformation? Invoice { get; set; }

        /// <summary> Lista de alertas transformadas genéricas (si reglas comunes las requieren) </summary>
        public List<Alert> Alerts { get; set; } = new();

        public InvoiceDifferentRadicates InvoiceDifferentRadicates { get; set; }

        public InvoiceDifferentRadicates PreviousObjections { get; set; }

        public InvoiceDifferentRadicates MultipleTransposrts { get; set; }

        public ValidationAggregationRules_31_40 ValidationAggregationRules_31_40 { get; set; }

        /// <summary> Solicitud de investigación (ResearchRequest aggregation) </summary>
        public ResearchRequest ResearchRequest { get; set; }

        /// <summary> Tipo de vehículo (Claim.Vehicle.Type.Value) </summary>
        public string VehicleType { get; set; } = string.Empty;

        /// <summary> Snapshot de datos del proveedor corporativo (catálogo interno) </summary>
        public ProviderData? ProviderData { get; set; }

        /// <summary> Lista de códigos de servicios (Services[].ServiceInfo.Code) para reglas transversales </summary>
        public List<string>? ListServiceCodes { get; set; }

        /// <summary> Errores de obligatoriedad detectados por el validador de modelo </summary>
        public List<string>? NotNullErrorsInModel { get; set; } = new();

        /// <summary> Errores de formato/tipo detectados por el validador de modelo </summary>
        public List<string>? TypeErrorsInModel { get; set; } = new();

        /// <summary> Mapa de tipificación que puede ser llenado por reglas (clave lógica -> descripción) </summary>
        public Dictionary<string, string> TypificationMap { get; set; } = new();

        /// <summary> Mapa de banderas de prioridad (clave de regla -> true/false) </summary>
        public Dictionary<string, bool> HasPriorityMap { get; set; } = new();
    }
}