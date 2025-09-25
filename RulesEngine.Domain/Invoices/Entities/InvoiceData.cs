using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using RulesEngine.Domain.Agregations.Entities;
using RulesEngine.Domain.ClaimsQueue.Entities;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.DisputeProcess.Entities;
using RulesEngine.Domain.ElectronicBillingRuleEngine.Entities;
using RulesEngine.Domain.Helper;
using RulesEngine.Domain.LegalProceedings.Entities;
using RulesEngine.Domain.Mundial.Invoices.Helper;
using RulesEngine.Domain.Provider.Entities;
using RulesEngine.Domain.Research.Entities;
using RulesEngine.Domain.TransactionContracts.Entities;

namespace RulesEngine.Domain.Invoices.Entities
{
    [BsonIgnoreExtraElements]
    public class InvoiceData
    {

        public string _id { get; set; } = string.Empty;
        public string RadNumber { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string? MainRadNumber { get; set; } = string.Empty;
        public string InvoiceOrigin { get; set; } = string.Empty;
        public string TypeIdentIps { get; set; } = string.Empty;
        public string NitIps { get; set; } = string.Empty;
        public string LoteIQ { get; set; } = string.Empty;
        public string NameIps { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public string HelpType { get; set; } = string.Empty;
        public string SucursalId { get; set; } = string.Empty;
        public string SucursalCityCode { get; set; } = string.Empty;
        public string SucursalMunicipality { get; set; } = string.Empty;
        public string SucursalCity { get; set; } = string.Empty;
        public string SucursalAddress { get; set; } = string.Empty;
        public string SucursalPhone { get; set; } = string.Empty;
        public List<SucursalEmail>? SucursalEmails { get; set; }
        public bool RipsV1 { get; set; }
        public bool RipsV2 { get; set; }
        public bool Furips { get; set; }
        public DateTime? DateReception { get; set; }
        public DateTime? DateRadication { get; set; }
        public DateTime? DateIntegration { get; set; }
        public DateTime? DateCreation { get; set; }
        public Sections? Sections { get; set; }
        public ElectronicBillingEntity? ElectronicBilling { get; set; }
        public Alert[]? Alerts { get; set; }
        public ResearchEntity[]? ResearchData { get; set; }
        public ClaimsQueueEntity ClaimsQueue { get; set; }
        public IEnumerable<ConsolidatedTransactionContracts>? ConsolidatedTransactionContracts { get; set; }
        public IEnumerable<LegalProceedingsEntity>? LegalProceedings { get; set; }
        public IEnumerable<DisputeProcessEntity>? LegalProcessesAndTransactionContractsParameters { get; set; }
        public InvoiceDifferentRadicates? InvoiceDifferentRadicates { get; set; }
        public ProviderData? ProviderData { get; set; }
        public SinisterAggregation? ValidationSinister { get; set; }
        public InvoiceDifferentRadicates? PreviousObjections { get; set; }
        public InvoiceDifferentRadicates? MultipleTransports { get; set; }
        public ResearchRequest? ResearchRequest { get; set; }
        public ValidationAggregationRules_31_40? ResultAggregationRules { get; set; }
        public List<string>? NotNullErrorsInModel { get; set; } = [];
        public List<string>? TypeErrorsInModel { get; set; } = [];
        public string InvoicePhoneVerificationValue { get; set; } = string.Empty;
        public string ParametrizedHelpType { get; set; } = string.Empty;
    }
    public class SucursalEmail
    {
        public string BasicEmail { get; set; } = string.Empty;
        public bool State { get; set; }
        public DateTime? NotificationDateCertimail { get; set; }
        public string NotificationType { get; set; } = string.Empty;
        public List<string> EmailsCc { get; set; } = new List<string>();
        public List<string> EmailsCco { get; set; } = new List<string>();
    }

    public class Sections
    {
        [BsonElement("claimData_furips1")]
        public ClaimDataFurips1? ClaimDataFurips1 { get; set; }

        [BsonElement("healthCareServiceProvider")]
        public HealthCareServiceProvider? HealthCareServiceProvider { get; set; }

        [BsonElement("victimData")]
        public VictimData? VictimData { get; set; }

        [BsonElement("catastrophicPlaceEvent")]
        public CatastrophicPlaceEvent? CatastrophicPlaceEvent { get; set; }

        [BsonElement("involvedVehicleInformation")]
        public InvolvedVehicleInformation? InvolvedVehicleInformation { get; set; }

        [BsonElement("dataRelatedVictim")]
        public DataRelatedVictim? DataRelatedVictim { get; set; }

        [BsonElement("ownerVehicle")]
        public OwnerVehicle? OwnerVehicle { get; set; }
        [BsonElement("driverInformation")]
        public DriverInformation? DriverInformation { get; set; }

        [BsonElement("remisionDate")]
        public RemissionDateTime? RemissionDate { get; set; }
        [BsonElement("mobilizationAndTransportationVictim")]
        public MobilizationAndTransportationVictim? MobilizationAndTransportationVictim { get; set; }

        [BsonElement("medicalCertification")]
        public MedicalCertification? MedicalCertification { get; set; }

        [BsonElement("doctorInformation")]
        public DoctorInformation? DoctorInformation { get; set; }

        [BsonElement("coveragesClaimed")]
        public CoveragesClaimed? CoveragesClaimed { get; set; }

        [BsonElement("claimData_furips2")]
        public ClaimDataFurips2? ClaimDataFurips2 { get; set; }

        [BsonElement("invoice")]
        public InvoiceInformation? Invoice { get; set; }
    }

    public class ClaimDataFurips1
    {
        [BsonElement("filingnumber")]
        public string Filingnumber { get; set; } = string.Empty;

        [BsonElement("lastfilingnumber")]
        public string Lastfilingnumber { get; set; } = string.Empty;

        [BsonElement("responseglossobjection")]
        public Deskriptor? Responseglossobjection { get; set; }

        [BsonElement("invoicenumber")]
        [BsonSerializer(typeof(StringOrIntToStringConverter))]
        public string InvoiceNumber { get; set; } = string.Empty;

        [BsonElement("consecutiveclaimnumber")]
        public long? Consecutiveclaimnumber { get; set; }
        [BsonElement("notificationdate")]
        public DateTime? Notificationdate { get; set; }
    }

    public class HealthCareServiceProvider
    {
        [BsonElement("healthcareserviceprovidercode")]
        [BsonSerializer(typeof(StringOrIntToStringConverter))]
        public string Healthcareserviceprovidercode { get; set; }
        [BsonElement("healthcarenit")]
        [BsonSerializer(typeof(StringOrIntToStringConverter))]
        public string Healthcarenit { get; set; } = string.Empty;
        [BsonElement("healthcareprovidername")]
        public string Healthcareprovidername { get; set; } = string.Empty;
    }

    public class VictimData
    {
        [BsonElement("firstlastname")]
        public string FirstLastName { get; set; } = string.Empty;
        [BsonElement("secondlastname")]
        public string SecondLastName { get; set; } = string.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("secondname")]
        public string SecondName { get; set; } = string.Empty;
        [BsonElement("documenttype")]
        public Deskriptor? DocumentType { get; set; }
        [BsonElement("identificationnumber")]
        public string IdentificationNumber { get; set; } = string.Empty;
        [BsonElement("birthdate")]
        public DateTime? BirthDate { get; set; }
        [BsonElement("deaddate")]
        public DateTime? DeadDate { get; set; }
        [BsonElement("sex")]
        public Deskriptor? Sex { get; set; }
        [BsonElement("address")]
        public string Address { get; set; } = string.Empty;
        [BsonElement("department")]
        public Deskriptor? Department { get; set; }
        [BsonElement("municipality")]
        public Municipality? Municipality { get; set; }
        [BsonElement("phone")]
        [BsonSerializer(typeof(StringOrIntToStringConverter))]
        public string? Phone { get; set; }
        [BsonElement("accidentcondition")]
        public Deskriptor? AccidentCondition { get; set; }
        [BsonElement("drivermunicipalitycode")]
        public string DriverMunicipalityCode { get; set; }
    }

    [BsonNoId]
    public class Municipality
    {
        public int? id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("dependentCode")]
        public string DependentCode { get; set; } = string.Empty;
    }

    public class CatastrophicPlaceEvent
    {
        [BsonElement("eventnature")]
        public Deskriptor? EventNature { get; set; }
        [BsonElement("accidentcause")]
        public Deskriptor? AccidentCause { get; set; }
        [BsonElement("adress")]
        public string Address { get; set; } = string.Empty;
        [BsonElement("eventdate")]
        public DateTime? EventDate { get; set; }
        [BsonElement("eventhour")]
        public string EventHour { get; set; } = string.Empty;
        [BsonElement("eventdepartment")]
        public Deskriptor? EventDepartment { get; set; }
        [BsonElement("eventmunicipality")]
        public EventMunicipality? EventMunicipality { get; set; }
        [BsonElement("eventzone")]
        public Deskriptor? EventZone { get; set; }
        [BsonElement("otherevent")]
        public string OtherEvent { get; set; } = string.Empty;
    }


    [BsonNoId]
    public class EventMunicipality
    {
        public int? id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("dependentCode")]
        public string DependentCode { get; set; } = string.Empty;
    }

    public class InvolvedVehicleInformation
    {
        [BsonElement("securestate")]
        public Deskriptor? SecureState { get; set; }
        [BsonElement("vehiclemake")]
        public string? VehicleMake { get; set; } = string.Empty;
        [BsonElement("licenseplate")]
        public string? LicensePlate { get; set; } = string.Empty;
        [BsonElement("vehicletype")]
        public Deskriptor? VehicleType { get; set; }
        [BsonElement("insurancecompanycode")]
        public string? InsuranceCompanyCode { get; set; } = string.Empty;
        [BsonElement("soatnumber")]
        public string? SoatNumber { get; set; }
        [BsonElement("initdatesoat")]
        public DateTime? InitDateSoat { get; set; }
        [BsonElement("enddatesoat")]
        public DateTime? EndDateSoat { get; set; }
        [BsonElement("sirasnumber")]
        public string? SirasNumber { get; set; } = string.Empty;
        [BsonElement("billinginsurelimit")]
        public string? BillingInsureLimit { get; set; }
    }

    public class DataRelatedVictim
    {
        [BsonElement("cupscode")]
        public string CupsCode { get; set; } = string.Empty;
        [BsonElement("procedurecomplexity")]
        public Deskriptor? ProcedureComplexity { get; set; }
        [BsonElement("maincupscode")]
        public string MainCupsCode { get; set; } = string.Empty;
        [BsonElement("secundarycupscode")]
        public string SecondaryCupsCode { get; set; } = string.Empty;
        [BsonElement("uciservice")]
        public Deskriptor? UciService { get; set; }
        [BsonElement("claimucidays")]
        public int? ClaimUciDays { get; set; }
    }

    public class OwnerVehicle
    {
        [BsonSerializer(typeof(DeskriptorSerializer))]
        [BsonElement("owneridentificationtype")]

        public Deskriptor? OwnerIdentificationType { get; set; }
        [BsonElement("owneridentificationnumber")]
        public object? OwnerIdentificationNumber { get; set; }
        [BsonElement("ownerfirstlastname")]
        public string OwnerFirstLastName { get; set; } = string.Empty;
        [BsonElement("ownersecondlastname")]
        public string OwnerSecondLastName { get; set; } = string.Empty;
        [BsonElement("ownerfirstname")]
        public string OwnerFirstName { get; set; } = string.Empty;
        [BsonElement("ownersecondname")]
        public string OwnerSecondName { get; set; } = string.Empty;
        [BsonElement("owneraddress")]
        public string OwnerAddress { get; set; } = string.Empty;
        [BsonElement("ownerhomephone")]
        [BsonSerializer(typeof(StringOrIntToStringConverter))]
        public string OwnerHomePhone { get; set; } = string.Empty;
        [BsonElement("ownerdepartmentcode")]
        public OwnerDepartmentCode? OwnerDepartmentCode { get; set; }
        [BsonElement("ownermunicipalitycode")]
        public OwnerMunicipalityCode? OwnerMunicipalityCode { get; set; }
    }

    [BsonNoId]
    public class OwnerDepartmentCode
    {
        [BsonElement("id")]
        public int? Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
    }

    [BsonNoId]
    public class OwnerMunicipalityCode
    {
        [BsonElement("id")]
        public int? Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("dependentCode")]
        public string DependentCode { get; set; } = string.Empty;
    }


    public class DriverInformation
    {
        [BsonElement("driverfirstlastname")]
        public string DriverFirstLastName { get; set; } = string.Empty;
        [BsonElement("driversecondlastname")]
        public string DriverSecondLastName { get; set; } = string.Empty;
        [BsonElement("driverfirstname")]
        public string DriverFirstName { get; set; } = string.Empty;
        [BsonElement("driversecondname")]
        public string DriverSecondName { get; set; } = string.Empty;
        [BsonElement("driveridentificationtype")]
        public DriverIdentificationType? DriverIdentificationType { get; set; }
        [BsonElement("driveridentificationnumber")]
        public string DriverIdentificationNumber { get; set; } = string.Empty;
        [BsonElement("driveraddress")]
        public string DriverAddress { get; set; } = string.Empty;
        [BsonElement("driverdepartmentcode")]
        public Deskriptor? DriverDepartmentCode { get; set; }
        [BsonElement("drivermunicipalitycode")]
        public DriverMunicipalityCode? DriverMunicipalityCode { get; set; }
        [BsonElement("driverphonenumber")]
        [BsonSerializer(typeof(StringOrIntToStringConverter))]
        public string? DriverPhoneNumber { get; set; } = string.Empty;
    }

    public class DriverIdentificationType
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("dependentCode")]
        public string DependentCode { get; set; } = string.Empty;
    }


    [BsonNoId]
    public class DriverMunicipalityCode
    {
        [BsonElement("id")]
        public int? Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("dependentCode")]
        public string DependentCode { get; set; } = string.Empty;
    }

    public class RemissionDateTime
    {
        [BsonElement("referencetype")]
        public Deskriptor? ReferenceType { get; set; }
        [BsonElement("remissiondate")]
        public DateTime? RemissionDate { get; set; }
        [BsonElement("remissionouthour")]
        public string RemissionOutHour { get; set; } = string.Empty;
        [BsonElement("remissionhabilitationcode")]
        public string RemissionHabilitationCode { get; set; } = string.Empty;
        [BsonElement("remissionprofessionalrefer")]
        public string RemissionProfessionalRefer { get; set; } = string.Empty;
        [BsonElement("remissioncharge")]
        public string RemissionCharge { get; set; } = string.Empty;
        [BsonElement("remissionaceptdate")]
        public DateTime? RemissionAcceptDate { get; set; }
        [BsonElement("remissionacceptationhour")]
        public string RemissionAcceptationHour { get; set; } = string.Empty;
        [BsonElement("remissionaceptationcode")]
        public string RemissionAcceptationCode { get; set; } = string.Empty;
        [BsonElement("remisionprofesionalaccept")]
        public string RemissionProfessionalAccept { get; set; } = string.Empty;
        [BsonElement("remissionambulanceplate")]
        public string RemissionAmbulancePlate { get; set; } = string.Empty;
    }

    public class MobilizationAndTransportationVictim
    {
        [BsonElement("primarytransferambulanceplate")]
        public string PrimaryTransferAmbulancePlate { get; set; } = string.Empty;
        [BsonElement("victimtransportationevent")]
        public string VictimTransportationEvent { get; set; } = string.Empty;
        [BsonElement("victimtransportationendjourney")]
        public string VictimTransportationEndJourney { get; set; } = string.Empty;
        [BsonElement("transportservicetype")]
        public TransportServiceType? TransportServiceType { get; set; }
        [BsonElement("victimarea")]
        public Deskriptor? VictimArea { get; set; }
    }

    public class TransportServiceType
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
    }

    public class MedicalCertification
    {
        [BsonElement("medicalcertificationincomedate")]
        public DateTime? MedicalCertificationIncomeDate { get; set; }
        [BsonElement("medicalcertificationincomehour")]
        public string? MedicalCertificationIncomeHour { get; set; }
        [BsonElement("medicalcertificationegressdate")]
        public DateTime? MedicalCertificationEgressDate { get; set; }
        [BsonElement("medicalcertificationouthour")]
        public string? MedicalCertificationOutHour { get; set; }
        [BsonElement("medicalcertificatiodiagnosisincome")]
        public Deskriptor? MedicalCertificationDiagnosisIncome { get; set; }
        [BsonElement("medicalcertificatiodiagnosisincome1")]
        public Deskriptor? MedicalCertificationDiagnosisIncome1 { get; set; }
        [BsonElement("medicalcertificatiodiagnosisincome2")]
        public object? MedicalCertificationDiagnosisIncome2 { get; set; }
        [BsonElement("medicalcertificatiodiagnosisegress")]
        public Deskriptor? MedicalCertificationDiagnosisEgress { get; set; }
        [BsonElement("medicalcertificatiodiagnosisegress1")]
        public object? MedicalCertificationDiagnosisEgress1 { get; set; }
        [BsonElement("medicalcertificatiodiagnosisegress2")]
        public object? MedicalCertificationDiagnosisEgress2 { get; set; }
    }

    public class DoctorInformation
    {
        [BsonElement("doctorfirstlastname")]
        public string DoctorFirstLastName { get; set; } = string.Empty;
        [BsonElement("doctorsecondlastname")]
        public string DoctorSecondLastName { get; set; } = string.Empty;
        [BsonElement("doctorfirstname")]
        public string DoctorFirstName { get; set; } = string.Empty;
        [BsonElement("doctorsecondname")]
        public string DoctorSecondName { get; set; } = string.Empty;
        [BsonElement("doctoridentificationtype")]
        public Deskriptor? DoctorIdentificationType { get; set; }
        [BsonElement("doctoridentificationnumber")]
        public string DoctorIdentificationNumber { get; set; } = string.Empty;
        [BsonElement("doctorregistrationnumber")]
        public string DoctorRegistrationNumber { get; set; } = string.Empty;
    }


    public class CoveragesClaimed
    {
        [BsonElement("totalbilledmedicalexpenses")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? TotalBilledMedicalExpenses { get; set; }
        [BsonElement("totalclaimmedicalexpenses")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? TotalClaimMedicalExpenses { get; set; }
        [BsonElement("totalbilledtransportation")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? TotalBilledTransportation { get; set; }
        [BsonElement("totalclaimtransportation")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? TotalClaimTransportation { get; set; }
    }

    public class ClaimDataFurips2
    {
        [BsonElement("claimconsecutivenumber")]
        [BsonSerializer(typeof(StringOrIntToStringConverter))]
        public string? ClaimConsecutiveNumber { get; set; }
        [BsonElement("reclaiminvoicenumber")]
        public string ReclaimInvoiceNumber { get; set; } = string.Empty;
        [BsonElement("claimdocumentdate")]
        public DateTime? ClaimDocumentDate { get; set; }
        [BsonElement("claimprovidernit")]
        [BsonSerializer(typeof(StringOrIntToStringConverter))]
        public string ClaimProviderNit { get; set; } = string.Empty;
        [BsonElement("claimprovidername")]
        public string ClaimProviderName { get; set; } = string.Empty;
        [BsonElement("claiminvoiceoffice")]
        public string ClaimInvoiceOffice { get; set; } = string.Empty;
        [BsonElement("claiminvoicevalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ClaimInvoiceValue { get; set; }
        [BsonElement("AccidentNumber")]
        public string AccidentNumber { get; set; } = string.Empty;
        [BsonElement("ReservedValue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ReservedValue { get; set; }
        [BsonElement("CoverageLimit")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? CoverageLimit { get; set; }
        [BsonElement("filingnumber")]
        public string FilingNumber { get; set; } = string.Empty;
    }


    public class InvoiceInformation
    {
        [BsonElement("servicelist")]
        public ServiceList[] ServiceList { get; set; } = Array.Empty<ServiceList>();
        [BsonElement("lib-glosses")]
        public string LibGlosses { get; set; } = string.Empty;
        [BsonElement("glossdetails")]
        public GlossDetail[] GlossDetails { get; set; } = Array.Empty<GlossDetail>();
        [BsonElement("glosstotals")]
        public GlossTotal[] GlossTotals { get; set; } = Array.Empty<GlossTotal>();
        [BsonElement("glossobservations")]
        public object? GlossObservations { get; set; }
    }

    public class ServiceList
    {
        [BsonElement("serviceconsecutive")]
        public long? ServiceConsecutive { get; set; }
        [BsonElement("servicetype")]
        [BsonSerializer(typeof(DeskriptorSerializer))]
        public Deskriptor? ServiceType { get; set; }
        [BsonElement("servicedescription")]
        [BsonSerializer(typeof(DeskriptorSerializer))]
        public Deskriptor? ServiceDescription { get; set; }
        [BsonElement("servicequantity")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ServiceQuantity { get; set; }
        [BsonElement("serviceunitvalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ServiceUnitValue { get; set; }
        [BsonElement("servicetotalbilled")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ServiceTotalBilled { get; set; }
        [BsonElement("ratevalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? RateValue { get; set; }
        [BsonElement("ratedifference")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? RateDifference { get; set; }
        [BsonElement("glossquantity")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? GlossQuantity { get; set; }
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        [BsonElement("glossunitvalue")]
        public long? GlossUnitValue { get; set; }
        [BsonElement("glosstotalvalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? GlossTotalValue { get; set; }
        [BsonElement("approvedquantity")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ApprovedQuantity { get; set; }
        [BsonElement("approvedunitvalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ApprovedUnitValue { get; set; }
        [BsonElement("approvedtotalvalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ApprovedTotalValue { get; set; }
        [BsonElement("action")]
        public string Action { get; set; } = string.Empty;
        [BsonElement("stylegloss")]
        public bool StyleGloss { get; set; }
        [BsonElement("servicetotalclaim")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ServiceTotalClaim { get; set; }
        [BsonElement("homologationdescription")]
        public Deskriptor? HomologationDescription { get; set; }
        [BsonElement("mosData")]
        public MosData? MosData { get; set; }

        [BsonElement("tariffhomologateuser")]
        public string? TariffHomologateUser { get; set; } = string.Empty;
        [BsonElement("IsActiveServiceForExport")]
        public bool? IsActiveServiceForExport { get; set; }
        [BsonElement("glossValueExport")]
        public int GlossValueExport { get; set; }
        [BsonElement("servicebilledExport")]
        public int ServiceBilledExport { get; set; }
    }

    public class ServiceType
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("state")]
        public string State { get; set; } = string.Empty;
    }

    public class ServiceDescription
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("state")]
        public bool State { get; set; }
    }
    public class HomologationDescription
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("state")]
        public string state { get; set; } = string.Empty;
    }

    public class MosData
    {
        [BsonElement("providerinvoice")]
        public object? ProviderInvoice { get; set; }
        [BsonElement("providernit")]
        [BsonSerializer(typeof(StringOrIntToStringConverter))]
        public string? ProviderNit { get; set; }
        [BsonElement("providername")]
        public string ProviderName { get; set; } = string.Empty;
        [BsonElement("providerinvoicenumber")]
        public string ProviderInvoiceNumber { get; set; } = string.Empty;
        [BsonElement("providerinvoicedate")]
        public string? ProviderInvoiceDate { get; set; }
        [BsonElement("principalservicecode")]
        public string PrincipalServiceCode { get; set; } = string.Empty;
        [BsonElement("serviceprovidermaterial")]
        public ServiceProviderMaterial[] ServiceProviderMaterial { get; set; } = Array.Empty<ServiceProviderMaterial>();
        [BsonElement("index")]
        public int? Index { get; set; }
    }

    public class ServiceProviderMaterial
    {
        [BsonElement("referencialmaterialcode")]
        public string ReferentialMaterialCode { get; set; } = string.Empty;
        [BsonElement("descriptionmaterial")]
        public string DescriptionMaterial { get; set; } = string.Empty;
        [BsonElement("quantitymaterial")]
        public object? QuantityMaterial { get; set; }
        [BsonElement("unitvaluematerial")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? UnitValueMaterial { get; set; }
        [BsonElement("totalmaterial")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? TotalMaterial { get; set; }
        [BsonElement("ivavaluematerial")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? IvaValueMaterial { get; set; }
        [BsonElement("totalbilledmaterial")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? TotalBilledMaterial { get; set; }
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        [BsonElement("consecutive")]
        public long? Consecutive { get; set; }
    }


    public class GlossDetail
    {
        [BsonElement("serviceconsecutive")]
        public long? ServiceConsecutive { get; set; }
        [BsonElement("glosscode")]
        public Deskriptor? GlossCode { get; set; }
        [BsonElement("generalgloss")]
        public Deskriptor? GeneralGloss { get; set; }
        [BsonElement("specificgloss")]
        public Deskriptor? SpecificGloss { get; set; }
        [BsonElement("observationdesc")]
        public string ObservationDesc { get; set; } = string.Empty;
        [BsonElement("glossquantity")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? GlossQuantity { get; set; }
        [BsonElement("glossunitvalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? GlossUnitValue { get; set; }
        [BsonElement("glosstotalvalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? GlossTotalValue { get; set; }
        [BsonElement("glossDate")]
        public string GlossDate { get; set; } = string.Empty;
        [BsonElement("glossUser")]
        public string GlossUser { get; set; } = string.Empty;
        [BsonElement("addgloss2")]
        public AddGloss2? AddGloss2 { get; set; }
        [BsonElement("created")]
        public bool Created { get; set; }
        [BsonElement("glossdescription")]
        public GlossDescription? GlossDescription { get; set; }
        [BsonElement("completeGlossCode")]
        public string CompleteGlossCode { get; set; } = string.Empty;
        [BsonElement("servicecode")]
        public string ServiceCode { get; set; } = string.Empty;
        [BsonElement("showDeleteGloss")]
        public bool ShowDeleteGloss { get; set; }
        [BsonElement("showEdit")]
        public bool EhowEdit { get; set; }
    }


    [BsonNoId]
    public class AddGloss2
    {
        [BsonElement("objectiontype")]
        public ObjectionType? ObjectionType { get; set; }

        [BsonElement("quantity")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? Quantity { get; set; }

        [BsonElement("percentage")]
        [BsonSerializer(typeof(StringOrIntToStringConverter))]
        public string? Percentage { get; set; } = string.Empty;

        [BsonElement("objectedunitvalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ObjectedUnitValue { get; set; }

        [BsonElement("objectedtotalvalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ObjectedTotalValue { get; set; }

        [BsonElement("observationType")]
        public ObservationType? ObservationType { get; set; }

        [BsonElement("observationdesc")]
        public string ObservationDesc { get; set; } = string.Empty;
    }

    [BsonNoId]
    public class ObjectionType
    {
        public int? id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("dependentCode")]
        public object? DependentCode { get; set; }
        [BsonElement("state")]
        public bool State { get; set; }
    }

    //[BsonNoId]
    public class ObservationType
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int? _id { get; set; }
        public int? id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("dependentCode")]
        public object? DependentCode { get; set; }
    }


    public class GlossDescription
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int? _id { get; set; }
        public int? id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("dependentCode")]
        public string DependentCode { get; set; } = string.Empty;
    }

    public class GlossTotal
    {
        [BsonElement("invoicetotal")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? InvoiceTotal { get; set; }
        [BsonElement("Approvedvalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? ApprovedValue { get; set; }
        [BsonElement("glossvalue")]
        [BsonSerializer(typeof(StringOrIntToLongConverter))]
        public long? GlossValue { get; set; }
        [BsonElement("approvaltype")]
        public string ApprovalType { get; set; } = string.Empty;
    }
    public class AccidentValidation
    {
        public string _id { get; set; } = string.Empty;
        public int Count { get; set; }
        public string[]? RadNumbers { get; set; }
    }



    public class Request
    {
        [BsonElement("investigationtype")]
        public Deskriptor? InvestigationType { get; set; }
        [BsonElement("researchfirm")]
        public Deskriptor? ResearchFirm { get; set; }
        [BsonElement("observationresearch")]
        public string ObservationResearch { get; set; } = string.Empty;
        [BsonElement("radNumber")]
        public string RadNumber { get; set; } = string.Empty;
        [BsonElement("soatnumber")]
        public long? SoatNumber { get; set; }
        [BsonElement("licenseplate")]
        public string LicensePlate { get; set; } = string.Empty;
        [BsonElement("documenttype")]
        public Deskriptor? DocumentType { get; set; }
        [BsonElement("identificationnumber")]
        public string IdentificationNumber { get; set; } = string.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("firstlastname")]
        public string FirstLastName { get; set; } = string.Empty;
        [BsonElement("helpType")]
        public string HelpType { get; set; } = string.Empty;
        [BsonElement("requestdateinvestigation")]
        public string RequestDateInvestigation { get; set; } = string.Empty;
        [BsonElement("healthcarenit")]
        public string HealthcareNit { get; set; } = string.Empty;
        [BsonElement("healthcareprovidername")]
        public string HealthcareProviderName { get; set; } = string.Empty;
        [BsonElement("action")]
        public string Action { get; set; } = string.Empty;
        [BsonElement("eventdate")]
        public DateTime? EventDate { get; set; }
        [BsonElement("sucursalcity")]
        public string SucursalCity { get; set; } = string.Empty;
        [BsonElement("sucursalmunicipality")]
        public string SucursalMunicipality { get; set; } = string.Empty;
        [BsonElement("pdfFilePath")]
        public string PdfFilePath { get; set; } = string.Empty;
        [BsonElement("VehicleType")]
        public string VehicleType { get; set; } = string.Empty;
        [BsonElement("InsuranceCompany")]
        public string InsuranceCompany { get; set; } = string.Empty;
        [BsonElement("IpsAdmissionDate")]
        public string IpsAdmissionDate { get; set; } = string.Empty;
    }

    [BsonIgnoreExtraElements]
    public class Deskriptor
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("value")]
        public string Value { get; set; } = string.Empty;
        [BsonElement("state")]
        private bool State { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ResponseResearchs
    {
        [BsonElement("Answer")]
        public AnswerResponse? Answer { get; set; }
        [BsonElement("Decline")]
        public DeclineResponse? Decline { get; set; }
        [BsonElement("Telephonic")]
        public TelephonicResponse? Telephonic { get; set; }
        public ExpirationResponseResearch Expiration { get; set; }
    }

    public class ExpirationResponseResearch
    {
        [BsonElement("observation")]
        public string Observation { get; set; }
        public string expirationDate { get; set; }
        public string processedBy { get; set; }
        public string reason { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class AnswerResponse
    {
        [BsonElement("casenumber")]
        public string CaseNumber { get; set; } = string.Empty;
        [BsonElement("state")]
        public Deskriptor? State { get; set; }
        [BsonSerializer(typeof(DeskriptorSerializer))]
        [BsonElement("typenotcovered")]
        public Deskriptor? TypeNotCovered { get; set; }
        [BsonElement("observation")]
        public string Observation { get; set; } = string.Empty;
        [BsonElement("File")]
        public string File { get; set; } = string.Empty;
    }

    public class DeclineResponse
    {
        [BsonElement("reason")]
        public Deskriptor? Reason { get; set; }
        [BsonElement("observation")]
        public string Observation { get; set; } = string.Empty;
    }
    public class TelephonicResponse
    {
        [BsonElement("observation")]
        public string Observation { get; set; } = string.Empty;
    }


    public class DeskriptorWithStateAndCode
    {

        [BsonRepresentation(BsonType.Int32)]
        [BsonElement("_Id")]
        public int? _Id { get; set; }
        public int? id { get; set; }
        [BsonElement("Code")]
        public string Code { get; set; } = string.Empty;
        [BsonElement("Description")]
        public string Description { get; set; } = string.Empty;
        [BsonElement("State")]
        public bool State { get; set; }
    }
}