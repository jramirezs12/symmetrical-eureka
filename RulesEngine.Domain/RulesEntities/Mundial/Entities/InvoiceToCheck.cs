using RulesEngine.Application.DataSources;
using RulesEngine.Domain.Agregations.Entities;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.DisputeProcess.Entities;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Primitives;
using RulesEngine.Domain.Provider.Entities;
using RulesEngine.Domain.Research.Entities;
using RulesEngine.Domain.ValueObjects;


namespace RulesEngine.Domain.RulesEntities.Mundial.Entities
{
    public partial class InvoiceToCheck : InputSourcesEntitty, IInvoiceToCheckContext, IRuleMetadataAware
    {
        public string RadNumber { get; private set; }
        private readonly IExternalDataLoader _loader;

        public InvoiceToCheck(string radNumber, IExternalDataLoader loader)
        {
            
            RadNumber = radNumber;
            _loader = loader;
        }

        public enum DocumentTypeEnum { CC = 1, DE, NIT, TI, PA, TSS, SEN, FDI, RC, AS, MS, TP, PE, PT, CN, SC, CD }
        //public string RadNumber { get; set; }
        public string IpsNit { get; set; }
        public string ModuleName { get; set; }

        /// <summary>
        /// Furisp1, involvedVehicleInformation, soatnumber =  poliza
        /// </summary>
        public string SoatNumber { get; set; }

        /// <summary>
        /// Furisp1, involvedVehicleInformation, licenseplate = placa
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// Furisp1, victimData, identificationnumber = número de documento de identidad
        /// </summary>
        public string VictimId { get; set; }

        /// <summary>
        /// Furisp1, victimData, DocumentType, value = tipo de documento
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Furisp1, catastrophicPlaceEvent, EventDate = Fecha de ocurrencia del evento
        /// </summary>
        public Date EventDate { get; set; }

        /// <summary>
        /// Furisp1, victimData, DeadDate = Fecha en caso de muerte
        /// </summary>
        public Date DeathDate { get; set; }

        /// <summary>
        /// Furisp1, claimData, NotificationDate = fecha de reclamación
        /// </summary>
        public Date ClaimDate { get; set; }

        /// <summary>
        /// Furisp2, claimData, claimdocumentdate = fecha de facturación
        /// </summary>
        public Date InvoiceDate { get; set; }

        /// <summary>
        /// Furisp1, medicalCertification, medicalcertificationincomedate = fecha ingreso
        /// </summary>
        public Date IncomeDate { get; set; }

        /// <summary>
        /// Furisp1, medicalCertification, medicalcertificationegressdate = fecha egreso
        /// </summary>
        public Date EgressDate { get; set; }

        /// <summary>
        /// Furisp1, claimData, invoicenumber = número de factura furisp1
        /// </summary>
        public string InvoiceNumberF1 { get; set; }

        /// <summary>
        /// Furisp2, claimData, reclaiminvoicenumber = = número de factura furisp2
        /// </summary>
        public string InvoiceNumberF2 { get; set; }

        /// <summary>
        /// Furisp1, remisionDate, remisionDate = fecha transporte primario*****
        /// </summary>
        public Date PrimaryTransportationDate { get; set; }

        /// <summary>
        /// Furips2, invoice, servicelist, arrays[], mosData, providerinvoicedate = Fecha factura proveedor MAOS
        /// </summary>
        public Date InvoiceMAOSDate { get; set; }

        /// <summary>
        /// Research, Response Date = fecha de resultado de la investigación
        /// </summary>
        public Date InvestigationResponseDate { get; set; }

        /// <summary>
        /// Furips2, ClaimData, claimprovidernit
        /// </summary>
        public string IpsNitF2 { get; set; }

        /// <summary>
        /// furips2, claimData, claiminvoicevalue = Valor factura
        /// </summary>
        public Currency InvoiceValue { get; set; }

        /// <summary>
        /// furips1, coveragesClaimed, totalbilledmedicalexpenses = Total facturado gastos médicos
        /// </summary>
        public Currency BilledMedicalExpenses { get; set; }

        /// <summary>
        /// furips1, coveragesClaimed, totalbilledtransportation = Total facturado transporte
        /// </summary>
        public Currency BilledTransportation { get; set; }

        /// <summary>
        /// InvoiceRips collection
        /// </summary>
        public string IpsNitRips { get; set; }

        /// <summary>
        /// InvoiceRips collection
        /// </summary>
        public string InvoiceNumberRips { get; set; }

        /// <summary>
        /// InvoiceRips collection
        /// </summary>
        public string IpsNitFurips { get; set; }

        /// <summary>
        /// InvoiceRips collection
        /// </summary>
        public string InvoiceNumberFurips { get; set; }

        /// <summary>
        /// Invoice FE collection
        /// </summary>
        public string IpsNitFE { get; set; }

        /// <summary>
        /// Invoice FE collection
        /// </summary>
        public string InvoiceNumberFE {  get; set; }


        /// <summary>
        /// InvoicePhoneVerificationValue - Parametro para validar si el  siniestro requiere verificación telefonica  (se valdia con el valor de la factura)
        /// </summary>
        public Currency InvoicePhoneVerificationValue { get; set; }

        /// <summary>
        /// Number of ocurrencies when the license plate has a multiple different event dates in the Invoice collection
        /// </summary>
        public int SamePlateDifferentEventNumber { get; set; }

        /// <summary>
        /// Number of ocurrencies when the license plate has a multiple different event dates, and the vehicle is 10(Motorcylce) in the Invoice collection
        /// </summary>
        public int SamePlateForMotorcycleDifferentEventNumber { get; set; }

        /// <summary>
        /// Number of ocurrencies when the VictimId has a multiple different event dates in the Invoice collection
        /// </summary>
        public int SameVictimIdDifferentEventNumber { get; set; }

        /// <summary>
        /// Esa propieda almacena el valor total glosado de la factura
        /// </summary>
        public Currency TotalGlossedValue { get; set; }
        /// <summary>
        /// Dato para validar si el valor total de la factura es igual al valor parametrizado
        /// </summary>
        public Currency TotalAuthorizedValue { get; set; }

        /// <summary>
        /// Amparo Médico
        /// </summary>
        public string HelpType { get; set; }
        /// <summary>
        /// Validación para Amparo Médico
        /// </summary>
        public string HelpTypeToValidate { get; set; }

        /// <summary>
        /// Usuario que hizo la reclamación
        /// </summary>
        public string UserClaim { get; set; }

        /// <summary>
        /// Número de placa de ambulancia
        /// </summary>
        public string LicensePlateAmbulance { get; set; }

        /// <summary>
        /// Número de factura de la reclamación
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Validacion de siniestros en diferentes radicados
        /// </summary>
        public SinisterAggregation SinisterAggretation { get; set; }

        /// <summary>
        /// Validacion de procedimientos legales - archivo matriz
        /// </summary>
        public List<DisputeProcessEntity> ProcessAndContracts { get; set; }

        /// <summary>
        /// Lista de alertas que existen en base de datos para el radicado en validación
        /// </summary>
        public Alert[] AlertsEncountered { get; set; }

        /// <summary>
        /// Contiene los resultados de investigación
        /// </summary>
        public ResearchEntity[] Research { get; set; }

        /// <summary>
        ///  Información de glosas
        /// </summary>
        public InvoiceInformation? Invoice { get; set; }

        /// <summary>
        /// List of alerts
        /// </summary>
        public List<Alert> Alerts { get; set; } = [];

        /// <summary>
        /// Validacion de facturas en diferentes radicados
        /// </summary>
        public InvoiceDifferentRadicates InvoiceDifferentRadicates { get; set; }
        /// <summary>
        /// Validacion Objeciones previas
        /// </summary>
        public InvoiceDifferentRadicates PreviousObjections { get; set; }
        /// <summary>
        /// Valdiacion Multiples transportes
        /// </summary>
        public InvoiceDifferentRadicates MultipleTransposrts { get; set; }
        public ValidationAggregationRules_31_40 ValidationAggregationRules_31_40 { get; set; }
        public string VehicleType { get; set; }

        /// <summary>
        /// Validacion de siniestros y resultados de einvestigacion
        /// </summary>
        public ResearchRequest ResearchRequest { get; set; }

        public ProviderData? ProviderData { get; set; }
        public List<string>? ListServiceCodes { get; set; }
        public List<string>? NotNullErrorsInModel { get; set; } = [];
        public List<string>? TypeErrorsInModel { get; set; } = [];
        public Dictionary<string, string> TypificationMap { get; set; } = new();
        public Dictionary<string, bool> HasPriorityMap { get; set; } = new();
    }
}
