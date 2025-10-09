using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

// Referencias a entidades ya existentes en tu dominio
using RulesEngine.Domain.Agregations.Entities;                  // SinisterAggregation, InvoiceDifferentRadicates, ValidationAggregationRules_31_40
using RulesEngine.Domain.ElectronicBillingRuleEngine.Entities;  // ElectronicBillingEntity
using RulesEngine.Domain.Provider.Entities;                     // ProviderData
using RulesEngine.Domain.ClaimsQueue.Entities;                  // ClaimsQueueEntity
using RulesEngine.Domain.Research.Entities;                     // ResearchEntity, ResearchRequest
using RulesEngine.Domain.LegalProceedings.Entities;             // LegalProceedingsEntity
using RulesEngine.Domain.TransactionContracts.Entities;         // ConsolidatedTransactionContracts
using RulesEngine.Domain.DisputeProcess.Entities;               // DisputeProcessEntity

namespace RulesEngine.Domain.Invoices.Entities
{
    /// <summary>
    /// Modelo completo para el esquema de facturas / reclamaciones del tenant Solidaria.
    /// IMPORTANTE:
    /// - NO hereda de InvoiceData para evitar colisiones de mapeo BSON (InvoiceNumber, RadNumber, etc.).
    /// - Sólo mapear aquí las propiedades realmente presentes en el documento JSON de Solidaria.
    /// - Los campos de enriquecimiento (agregaciones, cálculos, datos externos) van con [BsonIgnore].
    /// </summary>
    /// 
    public class ClaimDataSection
    {
        // Mapea a: "ClaimData"
        public string RadNumber { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public long? InvoiceValue { get; set; }
        public DateTime? FillingDate { get; set; }

        public string NumberClaim { get; set; } = string.Empty;

        public long Consecutive { get; set; }
    }

    public class InvolvedVehicleInformationSection
    {
        // Mapea a: "InvolvedVehicleInformation"
        public string LicensePlate { get; set; } = string.Empty;
        public string SoatNumber { get; set; } = string.Empty;
        public string TypeValue { get; set; } = string.Empty;
        public DateTime? ValidityStartDate { get; set; }

        public DateTime? ValidityEndDate { get; set; }
        public string InsuranceCompany { get; set; } = string.Empty;
        public string InsuranceStatus { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }

    public class VictimDataSection
    {
        // Mapea a: "VictimData"
        public string IdentificationNumber { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public DateTime? DeathDate { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string FirstLastName { get; set; } = string.Empty;

        public string IdTypeCode { get; set; } = string.Empty;

        public string IdTypeValue { get; set; } = string.Empty;

        public string IdNumber { get; set; } = string.Empty;

        public string Condition { get; set; } = string.Empty;


        public long MedicalSurgicalExpenses { get; set; }

        public long TotalClaimed { get; set; }

        public long VictimTransportAndMobilizationExpenses { get; set; } 
    }

    public class MedicalCertificationSection
    {
        // Mapea a: "MedicalCertification"
        public DateTime? IncomeDate { get; set; }
        public DateTime? EgressDate { get; set; }

        public string EgressMainDiagnosis { get; set; } = string.Empty;
    }

    public class EventInformationSection
    {
        // Mapea a: "EventInformation"
        public DateTime? EventDate { get; set; }

        public string Nature { get; set; } = string.Empty;

        public string Cause { get; set; } = string.Empty;

        public string Adress { get; set; } = string.Empty;

        public DateTime Date { get; set; } 

        public string Hour { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Municipality { get; set; } = string.Empty;

        public string Zone { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

    }

    public class RemissionInfoSection
    {
        // Mapea a: "RemissionInfo"
        public string PrimaryTransferAmbulancePlate { get; set; } = string.Empty;
    }

    public class PaymentDetailsSection
    {
        // Mapea a: "PaymentDetails"
        public long? PaymentValue { get; set; }
        public long? ConsolidatedPaymentsValue { get; set; }
    }

    public class TotalGlossValuesSection
    {
        // Mapea a: "TotalGlossValues"
        public long? TotalInvoiceApprovedValue { get; set; }
        public long? TotalInvoiceObjectedValue { get; set; }
    }
    public class AggregationSections
    {
        [BsonElement("ClaimData")]
        public ClaimDataSection ClaimData { get; set; } = new();

        [BsonElement("InvolvedVehicleInformation")]
        public InvolvedVehicleInformationSection InvolvedVehicleInformation { get; set; } = new();

        [BsonElement("VictimData")]
        public VictimDataSection VictimData { get; set; } = new();

        [BsonElement("MedicalCertification")]
        public MedicalCertificationSection MedicalCertification { get; set; } = new();

        [BsonElement("EventInformation")]
        public EventInformationSection EventInformation { get; set; } = new();

        [BsonElement("RemissionInfo")]
        public RemissionInfoSection RemissionInfo { get; set; } = new();

        [BsonElement("PaymentDetails")]
        public PaymentDetailsSection PaymentDetails { get; set; } = new();

        [BsonElement("TotalGlossValues")]
        public TotalGlossValuesSection TotalGlossValues { get; set; } = new();
    }
    [BsonIgnoreExtraElements]
    //public class SolidariaInvoiceData
    //{

    //    [BsonId]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string Id { get; set; } = string.Empty;


    //    [BsonElement("RadNumber")]
    //    public string RadNumber { get; set; } = string.Empty;

    //    // Campos enriquecidos/transformados en la raíz del documento

    //    // ModuleName se agregó en $addFields
    //    [BsonElement("ModuleName")]
    //    public string ModuleName { get; set; } = string.Empty;

    //    // IpsNit se agregó en $addFields
    //    [BsonElement("IpsNit")]
    //    public string IpsNit { get; set; } = string.Empty;

    //    // InvoiceDate se agregó en $addFields (a partir de InvoiceEmissionDate)
    //    [BsonElement("InvoiceDate")]
    //    public DateTime? InvoiceDate { get; set; }

    //    // ClaimDate se agregó en $addFields (a partir de PaymentDetails.ClaimFormalizationDate)
    //    [BsonElement("ClaimDate")]
    //    public DateTime? ClaimDate { get; set; }

    //    // La clave principal para la deserialización
    //    [BsonElement("Sections")]
    //    public AggregationSections Sections { get; set; } = new();

    //    // Campos que el $project conservó del documento original
    //    // Asumo que WorkflowData y Alerts son nodos complejos no modificados

    //    // Si tus nodos Alerts y WorkflowData tienen una estructura compleja, 
    //    // podrías usar Dictionary<string, object> si no quieres tiparlos completamente.
    //    //[BsonElement("Alerts")]
    //    //public List<Dictionary<string, object>>? Alerts { get; set; }

    //    [BsonElement("WorkflowData")]
    //    public Dictionary<string, object>? WorkflowData { get; set; }

    //    // =========================================================================
    //    // PROPIEDADES IGNORADAS
    //    // Propiedades del documento original que ya NO están en la salida del $project
    //    // =========================================================================

    //    // Propiedades que probablemente NO estén en la salida del $project, por lo tanto, se ignoran.
    //    [BsonIgnore] public long? OriginalInvoiceValue { get; set; }
    //    [BsonIgnore] public string BusinessInvoiceStatus { get; set; } = string.Empty;



    //    // Mantenemos tus propiedades de enriquecimiento marcadas con BsonIgnore

    //    [BsonIgnore] public string HelpType => string.Empty;
    //    // ... (El resto de tus propiedades BsonIgnore deben ir aquí)

    //    [BsonIgnore] public List<string>? NotNullErrorsInModel { get; set; } = new();

    //    [BsonIgnore] public List<string>? TypeErrorsInModel { get; set; } = new();


    //    [BsonElement("InvoiceNumber")]
    //    public string InvoiceNumber { get; set; } = string.Empty;



    //    // ------------------ Campos específicos del JSON Solidaria ------------------

    //    [BsonElement("ProcessLine")]
    //    public CodeValue? ProcessLine { get; set; }

    //    [BsonElement("SubProcessLine")]
    //    public CodeValue? SubProcessLine { get; set; }

    //    [BsonElement("ProtectionType")]
    //    public CodeValue? ProtectionType { get; set; }

    //    [BsonElement("Transaction")]
    //    public string Transaction { get; set; } = string.Empty;

    //    [BsonElement("InsuranceRadNumber")]
    //    public string InsuranceRadNumber { get; set; } = string.Empty;

    //    [BsonElement("InsuranceFillingDate")]
    //    public DateTime? InsuranceFillingDate { get; set; }

    //    [BsonElement("FillingDate")]
    //    public DateTime? FillingDate { get; set; }

    //    [BsonElement("LastRadNumber")]
    //    public string LastRadNumber { get; set; } = string.Empty;

    //    [BsonElement("LastFilingDate")]
    //    public DateTime? LastFilingDate { get; set; }

    //    [BsonElement("ResponseRGO")]
    //    public CodeValue? ResponseRGO { get; set; }

    //    [BsonElement("Cufe")]
    //    public string Cufe { get; set; } = string.Empty;

    //    [BsonElement("InvoiceValue")]
    //    public long? InvoiceValue { get; set; }

    //    [BsonElement("InvoiceEmissionDate")]
    //    public DateTime? InvoiceEmissionDate { get; set; }

    //    [BsonElement("InvoiceReceptionDate")]
    //    public DateTime? InvoiceReceptionDate { get; set; }



    //    [BsonElement("Provider")]
    //    public ProviderNode? Provider { get; set; }

    //    [BsonElement("Claim")]
    //    public ClaimNode? Claim { get; set; }

    //    [BsonElement("Alerts")]
    //    public List<AlertNode>? AlertsNode { get; set; }

    //    //[BsonElement("WorkflowData")]
    //    //public WorkflowDataNode? WorkflowData { get; set; }

    //    [BsonElement("AuditProcessFlags")]
    //    public AuditProcessFlagsNode? AuditProcessFlags { get; set; }

    //    // ------------------ ENRIQUECIMIENTOS (NO PERSISTEN) ------------------
    //    [BsonIgnore] public string ParametrizedHelpType { get; set; } = string.Empty;
    //    [BsonIgnore] public string InvoicePhoneVerificationValue { get; set; } = string.Empty;
    //    [BsonIgnore] public ElectronicBillingEntity? ElectronicBilling { get; set; }
    //    [BsonIgnore] public SinisterAggregation? ValidationSinister { get; set; }
    //    [BsonIgnore] public InvoiceDifferentRadicates? InvoiceDifferentRadicates { get; set; }
    //    [BsonIgnore] public InvoiceDifferentRadicates? PreviousObjections { get; set; }
    //    [BsonIgnore] public InvoiceDifferentRadicates? MultipleTransports { get; set; }
    //    [BsonIgnore] public ResearchRequest? ResearchRequest { get; set; }
    //    [BsonIgnore] public ResearchEntity[]? ResearchData { get; set; }
    //    [BsonIgnore] public ClaimsQueueEntity? ClaimsQueue { get; set; }
    //    [BsonIgnore] public IEnumerable<ConsolidatedTransactionContracts>? ConsolidatedTransactionContracts { get; set; }
    //    [BsonIgnore] public IEnumerable<LegalProceedingsEntity>? LegalProceedings { get; set; }
    //    [BsonIgnore] public IEnumerable<DisputeProcessEntity>? LegalProcessesAndTransactionContractsParameters { get; set; }
    //    [BsonIgnore] public ValidationAggregationRules_31_40? ResultAggregationRules { get; set; }
    //    [BsonIgnore] public ProviderData? ProviderData { get; set; }



    //}

    public class SolidariaInvoiceData
    {
        // Campos proyectados directamente desde el documento original
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("RadNumber")]
        public string RadNumber { get; set; } = string.Empty;

        // Campos enriquecidos/transformados en la raíz del documento

        // ModuleName se agregó en $addFields
        [BsonElement("ModuleName")]
        public string ModuleName { get; set; } = string.Empty;

        // IpsNit se agregó en $addFields
        [BsonElement("IpsNit")]
        public string IpsNit { get; set; } = string.Empty;

        [BsonElement("ProtectionType")]
        public string ProtectionType { get; set; } = string.Empty;

        [BsonElement("HabilitationCode")]
        public string HabilitationCode { get; set; } = string.Empty;

        [BsonElement("NameIps")]
        public string NameIps { get; set; } = string.Empty;


        // InvoiceDate se agregó en $addFields (a partir de InvoiceEmissionDate)
        [BsonElement("InvoiceDate")]
        public DateTime? InvoiceDate { get; set; }

        // ClaimDate se agregó en $addFields (a partir de PaymentDetails.ClaimFormalizationDate)
        [BsonElement("ClaimDate")]
        public DateTime? ClaimDate { get; set; }

        // La clave principal para la deserialización
        [BsonElement("Sections")]
        public AggregationSections Sections { get; set; } = new();

        // Campos que el $project conservó del documento original
        // Asumo que WorkflowData y Alerts son nodos complejos no modificados

        // Si tus nodos Alerts y WorkflowData tienen una estructura compleja, 
        // podrías usar Dictionary<string, object> si no quieres tiparlos completamente.
        [BsonElement("Alerts")]
        //public List<Dictionary<string, object>>? Alerts { get; set; }

        public List<AlertNode>? AlertsNode { get; set; }

        [BsonElement("WorkflowData")]
        public Dictionary<string, object>? WorkflowData { get; set; }

        // =========================================================================
        // PROPIEDADES IGNORADAS
        // Propiedades del documento original que ya NO están en la salida del $project
        // =========================================================================

        // Propiedades que probablemente NO estén en la salida del $project, por lo tanto, se ignoran.
        [BsonIgnore] public long? OriginalInvoiceValue { get; set; }
        [BsonIgnore] public string BusinessInvoiceStatus { get; set; } = string.Empty;
        [BsonIgnore] public object? Provider { get; set; }
        [BsonIgnore] public object? Claim { get; set; }

        [BsonIgnore] public List<string>? NotNullErrorsInModel { get; set; } = new();

        [BsonIgnore] public List<string>? TypeErrorsInModel { get; set; } = new();

        [BsonIgnore] public ValidationAggregationRules_31_40? ResultAggregationRules { get; set; }

        // Mantenemos tus propiedades de enriquecimiento marcadas con BsonIgnore
        [BsonIgnore] public string ParametrizedHelpType { get; set; } = string.Empty;
        [BsonIgnore] public string HelpType => string.Empty;
        // ... (El resto de tus propiedades BsonIgnore deben ir aquí)
    }

    // ================== NODOS GENERALES ==================
    [BsonIgnoreExtraElements]
    public class CodeValue
    {
        [BsonElement("Code")] public string Code { get; set; } = string.Empty;
        [BsonElement("Value")] public string Value { get; set; } = string.Empty;
    }

    // ================== PROVIDER ==================
    [BsonIgnoreExtraElements]
    public class ProviderNode
    {
        [BsonElement("IdType")]
        public CodeValue? IdType { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; } = string.Empty;

        [BsonElement("HabilitationCode")]
        public string HabilitationCode { get; set; } = string.Empty;

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("Department")]
        public CodeValue? Department { get; set; }

        [BsonElement("City")]
        public CodeValue? City { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("PostalCode")]
        public string PostalCode { get; set; } = string.Empty;

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("Branch")]
        public CodeValue? Branch { get; set; }

        [BsonElement("Emails")]
        public List<ProviderEmailGroup>? Emails { get; set; }
    }

    public class ProviderEmailGroup
    {
        [BsonElement("Email")]
        public List<string>? Email { get; set; }

        [BsonElement("EmailsCc")]
        public List<string>? EmailsCc { get; set; }

        [BsonElement("EmailsCco")]
        public List<string>? EmailsCco { get; set; }
    }

    // ================== CLAIM RAÍZ ==================
    [BsonIgnoreExtraElements]
    public class ClaimNode
    {
        [BsonElement("Consecutive")]
        public int? Consecutive { get; set; }

        [BsonElement("Status")]
        public CodeValue? Status { get; set; }

        [BsonElement("Number")]
        public string Number { get; set; } = string.Empty;

        [BsonElement("FiscalYear")]
        public string FiscalYear { get; set; } = string.Empty;

        [BsonElement("PolicyBranch")]
        public string PolicyBranch { get; set; } = string.Empty;

        [BsonElement("EnabledServicesDemostration")]
        public string EnabledServicesDemostration { get; set; } = string.Empty;

        [BsonElement("EventDescription")]
        public string EventDescription { get; set; } = string.Empty;

        [BsonElement("ReservationInfo")]
        public ReservationInfoNode? ReservationInfo { get; set; }

        [BsonElement("ClaimantStatement")]
        public CodeValue? ClaimantStatement { get; set; }

        [BsonElement("ClaimantBeneficiary")]
        public ClaimBeneficiaryNode? ClaimantBeneficiary { get; set; }

        [BsonElement("Event")]
        public EventNode? Event { get; set; }

        [BsonElement("Vehicle")]
        public VehicleNode? Vehicle { get; set; }

        [BsonElement("Victims")]
        public List<VictimNode>? Victims { get; set; }

        [BsonElement("PaymentDetails")]
        public PaymentDetailsNode? PaymentDetails { get; set; }

        [BsonElement("TotalGlossValues")]
        public TotalGlossValuesNode? TotalGlossValues { get; set; }

        [BsonElement("Observations")]
        public List<ObservationNode>? Observations { get; set; }
    }

    public class ReservationInfoNode
    {
        [BsonElement("MaxValueProtection")]
        public long? MaxValueProtection { get; set; }

        [BsonElement("Value")]
        public long? Value { get; set; }
    }

    public class ClaimBeneficiaryNode
    {
        [BsonElement("Consecutive")]
        public int? Consecutive { get; set; }

        [BsonElement("BeneficiaryType")]
        public CodeValue? BeneficiaryType { get; set; }

        [BsonElement("IdType")]
        public CodeValue? IdType { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; } = string.Empty;

        [BsonElement("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("MiddleName")]
        public string MiddleName { get; set; } = string.Empty;

        [BsonElement("FirstLastName")]
        public string FirstLastName { get; set; } = string.Empty;

        [BsonElement("SecondLastName")]
        public string SecondLastName { get; set; } = string.Empty;

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("CellPhoneNumber")]
        public string CellPhoneNumber { get; set; } = string.Empty;

        [BsonElement("Email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("ResidenceInfo")]
        public ResidenceInfoNode? ResidenceInfo { get; set; }

        [BsonElement("VictimRelationship")]
        public CodeValue? VictimRelationship { get; set; }

        [BsonElement("VictimIdNumber")]
        public string VictimIdNumber { get; set; } = string.Empty;

        [BsonElement("BankAccountInfo")]
        public BankAccountInfoNode? BankAccountInfo { get; set; }

        [BsonElement("LegalRepresentativeInfo")]
        public List<LegalRepresentativeNode>? LegalRepresentativeInfo { get; set; }
    }

    public class ResidenceInfoNode
    {
        [BsonElement("Address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("Country")]
        public CodeValue? Country { get; set; }

        [BsonElement("Department")]
        public CodeValue? Department { get; set; }

        [BsonElement("Municipality")]
        public CodeValue? Municipality { get; set; }

        [BsonElement("Zone")]
        public CodeValue? Zone { get; set; }
    }

    public class BankAccountInfoNode
    {
        [BsonElement("Type")]
        public CodeValue? Type { get; set; }

        [BsonElement("AccountNumber")]
        public string AccountNumber { get; set; } = string.Empty;

        [BsonElement("BankName")]
        public string BankName { get; set; } = string.Empty;
    }

    public class LegalRepresentativeNode
    {
        [BsonElement("ProfessionalCardNumber")]
        public string ProfessionalCardNumber { get; set; } = string.Empty;

        [BsonElement("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("MiddleName")]
        public string MiddleName { get; set; } = string.Empty;

        [BsonElement("FirstLastName")]
        public string FirstLastName { get; set; } = string.Empty;

        [BsonElement("SecondLastName")]
        public string SecondLastName { get; set; } = string.Empty;

        [BsonElement("Gender")]
        public CodeValue? Gender { get; set; }

        [BsonElement("IdType")]
        public CodeValue? IdType { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; } = string.Empty;

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("Email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("ResidenceInfo")]
        public ResidenceInfoNode? ResidenceInfo { get; set; }
    }

    // ================== EVENTO ==================
    public class EventNode
    {
        [BsonElement("Nature")]
        public CodeValue? Nature { get; set; }

        [BsonElement("Cause")]
        public CodeValue? Cause { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("Date")]
        public DateTime? Date { get; set; }

        [BsonElement("Hour")]
        public string Hour { get; set; } = string.Empty;

        [BsonElement("Department")]
        public CodeValue? Department { get; set; }

        [BsonElement("Municipality")]
        public CodeValue? Municipality { get; set; }

        [BsonElement("Zone")]
        public CodeValue? Zone { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; } = string.Empty;
    }

    // ================== VEHÍCULO ==================
    public class VehicleNode
    {
        [BsonElement("InsuranceStatus")]
        public CodeValue? InsuranceStatus { get; set; }

        [BsonElement("Brand")]
        public string Brand { get; set; } = string.Empty;

        [BsonElement("PlateNumber")]
        public string PlateNumber { get; set; } = string.Empty;

        [BsonElement("AuthorityIntervention")]
        public bool? AuthorityIntervention { get; set; }

        [BsonElement("Type")]
        public CodeValue? Type { get; set; }

        [BsonElement("Soat")]
        public SoatNode? Soat { get; set; }

        [BsonElement("EducationalInstitution")]
        public EducationalInstitutionNode? EducationalInstitution { get; set; }

        [BsonElement("OwnerData")]
        public OwnerDataNode? OwnerData { get; set; }

        [BsonElement("DriverInvolved")]
        public DriverInvolvedNode? DriverInvolved { get; set; }
    }

    public class SoatNode
    {
        [BsonElement("InsuranceCompany")]
        public InsuranceCompanyNode? InsuranceCompany { get; set; }

        [BsonElement("Policy")]
        public PolicyNode? Policy { get; set; }

        [BsonElement("SIRASFilingNumber")]
        public string SIRASFilingNumber { get; set; } = string.Empty;
    }

    public class InsuranceCompanyNode
    {
        [BsonElement("Code")]
        public string Code { get; set; } = string.Empty;

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("LimitExhaustionCharge")]
        public CodeValue? LimitExhaustionCharge { get; set; }
    }

    public class PolicyNode
    {
        [BsonElement("Number")]
        public string Number { get; set; } = string.Empty;

        [BsonElement("ValidityStartDate")]
        public DateTime? ValidityStartDate { get; set; }

        [BsonElement("ValidityEndDate")]
        public DateTime? ValidityEndDate { get; set; }
    }

    public class EducationalInstitutionNode
    {
        [BsonElement("IdType")]
        public CodeValue? IdType { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; } = string.Empty;

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;
    }

    public class OwnerDataNode
    {
        [BsonElement("IdType")]
        public CodeValue? IdType { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; } = string.Empty;

        [BsonElement("CompanyName")]
        public string CompanyName { get; set; } = string.Empty;

        [BsonElement("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("MiddleName")]
        public string MiddleName { get; set; } = string.Empty;

        [BsonElement("FirstLastName")]
        public string FirstLastName { get; set; } = string.Empty;

        [BsonElement("SecondLastName")]
        public string SecondLastName { get; set; } = string.Empty;

        [BsonElement("ResidenceInfo")]
        public ResidenceInfoNode? ResidenceInfo { get; set; }
    }

    public class DriverInvolvedNode
    {
        [BsonElement("IdType")]
        public CodeValue? IdType { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; } = string.Empty;

        [BsonElement("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("MiddleName")]
        public string MiddleName { get; set; } = string.Empty;

        [BsonElement("FirstLastName")]
        public string FirstLastName { get; set; } = string.Empty;

        [BsonElement("SecondLastName")]
        public string SecondLastName { get; set; } = string.Empty;

        [BsonElement("ResidenceInfo")]
        public ResidenceInfoNode? ResidenceInfo { get; set; }
    }

    // ================== VÍCTIMAS ==================
    public class VictimNode
    {
        [BsonElement("IdType")]
        public CodeValue? IdType { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; } = string.Empty;

        [BsonElement("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("MiddleName")]
        public string MiddleName { get; set; } = string.Empty;

        [BsonElement("FirstLastName")]
        public string FirstLastName { get; set; } = string.Empty;

        [BsonElement("SecondLastName")]
        public string SecondLastName { get; set; } = string.Empty;

        [BsonElement("BirthDate")]
        public DateTime? BirthDate { get; set; }

        [BsonElement("Condition")]
        public CodeValue? Condition { get; set; }

        [BsonElement("Gender")]
        public CodeValue? Gender { get; set; }

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("CellPhoneNumber")]
        public string CellPhoneNumber { get; set; } = string.Empty;

        [BsonElement("ResidenceInfo")]
        public ResidenceInfoNode? ResidenceInfo { get; set; }

        [BsonElement("ProtectionsClaimed")]
        public ProtectionsClaimedNode? ProtectionsClaimed { get; set; }

        [BsonElement("DeathInfo")]
        public DeathInfoNode? DeathInfo { get; set; }

        [BsonElement("MedicalAttention")]
        public MedicalAttentionNode? MedicalAttention { get; set; }

        [BsonElement("Services")]
        public List<ServiceNode>? Services { get; set; }

        [BsonElement("Beneficiaries")]
        public List<BeneficiaryNode>? Beneficiaries { get; set; }

        [BsonElement("BeneficiariesLiquidation")]
        public List<BeneficiaryLiquidationNode>? BeneficiariesLiquidation { get; set; }

        [BsonElement("RemissionInfo")]
        public RemissionInfoNode? RemissionInfo { get; set; }
    }

    public class ProtectionsClaimedNode
    {
        [BsonElement("Protection")]
        public CodeValue? Protection { get; set; }

        [BsonElement("Coverage")]
        public CodeValue? Coverage { get; set; }

        [BsonElement("ClaimedValue")]
        public long? ClaimedValue { get; set; }

        [BsonElement("MedicalSurgicalExpenses")]
        public SimpleBilledClaimedNode? MedicalSurgicalExpenses { get; set; }

        [BsonElement("VictimTransportAndMobilizationExpenses")]
        public SimpleBilledClaimedNode? VictimTransportAndMobilizationExpenses { get; set; }

        [BsonElement("DisabilityExpenses")]
        public SimpleBilledClaimedNode? DisabilityExpenses { get; set; }

        [BsonElement("DeathAndFuneralExpenses")]
        public SimpleBilledClaimedNode? DeathAndFuneralExpenses { get; set; }

        [BsonElement("MedicalRefundExpenses")]
        public SimpleBilledClaimedNode? MedicalRefundExpenses { get; set; }
    }

    public class SimpleBilledClaimedNode
    {
        [BsonElement("TotalBilled")]
        public long? TotalBilled { get; set; }

        [BsonElement("TotalClaimed")]
        public long? TotalClaimed { get; set; }
    }

    public class DeathInfoNode
    {
        [BsonElement("DeathDate")]
        public DateTime? DeathDate { get; set; }

        [BsonElement("DeathRecord")]
        public string DeathRecord { get; set; } = string.Empty;

        [BsonElement("ProsecutorOfficeRadNumber")]
        public string ProsecutorOfficeRadNumber { get; set; } = string.Empty;
    }

    public class MedicalAttentionNode
    {
        [BsonElement("MainHospitalizationServiceCupsCode")]
        public string MainHospitalizationServiceCupsCode { get; set; } = string.Empty;

        [BsonElement("SurgicalProcedureComplexity")]
        public CodeValue? SurgicalProcedureComplexity { get; set; }

        [BsonElement("MainSurgicalProcedureCupsCode")]
        public string MainSurgicalProcedureCupsCode { get; set; } = string.Empty;

        [BsonElement("SecondarySurgicalProcedureCupsCode")]
        public string SecondarySurgicalProcedureCupsCode { get; set; } = string.Empty;

        [BsonElement("UCIService")]
        public bool? UCIService { get; set; }

        [BsonElement("UCIDaysClaimed")]
        public int? UCIDaysClaimed { get; set; }

        [BsonElement("IncomeDate")]
        public DateTime? IncomeDate { get; set; }

        [BsonElement("CheckInTime")]
        public string CheckInTime { get; set; } = string.Empty;

        [BsonElement("EgressDate")]
        public DateTime? EgressDate { get; set; }

        [BsonElement("CheckOutTime")]
        public string CheckOutTime { get; set; } = string.Empty;

        [BsonElement("IncomeMainDiagnosis")]
        public CodeValue? IncomeMainDiagnosis { get; set; }

        [BsonElement("AssociatedIncomeDiagnosis1")]
        public CodeValue? AssociatedIncomeDiagnosis1 { get; set; }

        [BsonElement("AssociatedIncomeDiagnosis2")]
        public CodeValue? AssociatedIncomeDiagnosis2 { get; set; }

        [BsonElement("EgressMainDiagnosis")]
        public CodeValue? EgressMainDiagnosis { get; set; }

        [BsonElement("AssociatedEgressDiagnosis1")]
        public CodeValue? AssociatedEgressDiagnosis1 { get; set; }

        [BsonElement("AssociatedEgressDiagnosis2")]
        public CodeValue? AssociatedEgressDiagnosis2 { get; set; }

        [BsonElement("HealthCareProfessionalInfo")]
        public HealthCareProfessionalInfoNode? HealthCareProfessionalInfo { get; set; }
    }

    public class HealthCareProfessionalInfoNode
    {
        [BsonElement("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("MiddleName")]
        public string MiddleName { get; set; } = string.Empty;

        [BsonElement("FirstLastName")]
        public string FirstLastName { get; set; } = string.Empty;

        [BsonElement("SecondLastName")]
        public string SecondLastName { get; set; } = string.Empty;

        [BsonElement("IdType")]
        public CodeValue? IdType { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; } = string.Empty;

        [BsonElement("RegistrationMedicalNumber")]
        public string RegistrationMedicalNumber { get; set; } = string.Empty;
    }

    // ================== SERVICIOS Y GLOSAS ==================
    public class ServiceNode
    {
        [BsonElement("Consecutive")]
        public int? Consecutive { get; set; }

        [BsonElement("ServiceType")]
        public CodeValue? ServiceType { get; set; }

        [BsonElement("ServiceInfo")]
        public CodeValue? ServiceInfo { get; set; }

        [BsonElement("UnitQuantity")]
        public long? UnitQuantity { get; set; }

        [BsonElement("UnitValue")]
        public long? UnitValue { get; set; }

        [BsonElement("TotalBilledValue")]
        public long? TotalBilledValue { get; set; }

        [BsonElement("TotalClaimedValue")]
        public long? TotalClaimedValue { get; set; }

        [BsonElement("ApprovedUnitQuantity")]
        public long? ApprovedUnitQuantity { get; set; }

        [BsonElement("ApprovedUnitValue")]
        public long? ApprovedUnitValue { get; set; }

        [BsonElement("TotalApprovedValue")]
        public long? TotalApprovedValue { get; set; }

        [BsonElement("GlossQuantity")]
        public long? GlossQuantity { get; set; }

        [BsonElement("GlossUnitValue")]
        public long? GlossUnitValue { get; set; }

        [BsonElement("GlossTotalValue")]
        public long? GlossTotalValue { get; set; }

        [BsonElement("RateValue")]
        public long? RateValue { get; set; }

        [BsonElement("TariffDifferenceValue")]
        public long? TariffDifferenceValue { get; set; }

        [BsonElement("Glosses")]
        public List<GlossNode>? Glosses { get; set; }

        [BsonElement("Authorization")]
        public AuthorizationNode? Authorization { get; set; }

        [BsonElement("OsteosynthesisMaterialData")]
        public OsteosynthesisMaterialDataNode? OsteosynthesisMaterialData { get; set; }
    }

    public class GlossNode
    {
        [BsonElement("Consecutive")]
        public int? Consecutive { get; set; }

        [BsonElement("Code")]
        public string Code { get; set; } = string.Empty;

        [BsonElement("Type")]
        public CodeValue? Type { get; set; }

        [BsonElement("Description")]
        public CodeValue? Description { get; set; }

        [BsonElement("Observation")]
        public string Observation { get; set; } = string.Empty;

        [BsonElement("UnitQuantity")]
        public long? UnitQuantity { get; set; }

        [BsonElement("UnitGlossedValue")]
        public long? UnitGlossedValue { get; set; }

        [BsonElement("TotalGlossedValue")]
        public long? TotalGlossedValue { get; set; }

        [BsonElement("GlossDate")]
        public DateTime? GlossDate { get; set; }

        [BsonElement("RegistryUser")]
        public string RegistryUser { get; set; } = string.Empty;

        [BsonElement("CompleteGlossCode")]
        public string CompleteGlossCode { get; set; } = string.Empty;

        [BsonElement("GeneralGloss")]
        public CodeValue? GeneralGloss { get; set; }

        [BsonElement("SpecificGloss")]
        public CodeValue? SpecificGloss { get; set; }

        [BsonElement("Percentage")]
        public long? Percentage { get; set; }

        [BsonElement("ObservationType")]
        public CodeValue? ObservationType { get; set; }

        [BsonElement("RegistryRole")]
        public string RegistryRole { get; set; } = string.Empty;

        [BsonElement("Responses")]
        public List<GlossResponseNode>? Responses { get; set; }
    }

    public class GlossResponseNode
    {
        [BsonElement("Type")]
        public CodeValue? Type { get; set; }

        [BsonElement("Response")]
        public string Response { get; set; } = string.Empty;

        [BsonElement("AcceptedValue")]
        public long? AcceptedValue { get; set; }

        [BsonElement("UnacceptedValue")]
        public long? UnacceptedValue { get; set; }

        [BsonElement("Observation")]
        public string Observation { get; set; } = string.Empty;

        [BsonElement("ResponseUser")]
        public string ResponseUser { get; set; } = string.Empty;

        [BsonElement("ResponseDate")]
        public DateTime? ResponseDate { get; set; }
    }

    public class AuthorizationNode
    {
        [BsonElement("ApeNumber")]
        public string ApeNumber { get; set; } = string.Empty;

        [BsonElement("TechnicalNumber")]
        public string TechnicalNumber { get; set; } = string.Empty;

        [BsonElement("Notification")]
        public AuthorizationNotificationNode? Notification { get; set; }

        [BsonElement("PaymentOrder")]
        public PaymentOrderNode? PaymentOrder { get; set; }
    }

    public class AuthorizationNotificationNode
    {
        [BsonElement("Identifier")]
        public string Identifier { get; set; } = string.Empty;

        [BsonElement("Email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("Date")]
        public DateTime? Date { get; set; }
    }

    public class PaymentOrderNode
    {
        [BsonElement("Number")]
        public string Number { get; set; } = string.Empty;

        [BsonElement("Date")]
        public DateTime? Date { get; set; }
    }

    public class OsteosynthesisMaterialDataNode
    {
        [BsonElement("IsInvoiceProvider")]
        public bool? IsInvoiceProvider { get; set; }

        [BsonElement("MaterialProvider")]
        public MaterialProviderNode? MaterialProvider { get; set; }

        [BsonElement("Materials")]
        public List<OsteoMaterialNode>? Materials { get; set; }
    }

    public class MaterialProviderNode
    {
        [BsonElement("Nit")]
        public string Nit { get; set; } = string.Empty;

        [BsonElement("CompanyName")]
        public string CompanyName { get; set; } = string.Empty;

        [BsonElement("InvoiceDate")]
        public DateTime? InvoiceDate { get; set; }

        [BsonElement("MainProcedureCode")]
        public string MainProcedureCode { get; set; } = string.Empty;
    }

    public class OsteoMaterialNode
    {
        [BsonElement("Consecutive")]
        public int? Consecutive { get; set; }

        [BsonElement("ReferenceCode")]
        public string ReferenceCode { get; set; } = string.Empty;

        [BsonElement("Description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("Quantity")]
        public long? Quantity { get; set; }

        [BsonElement("UnitValue")]
        public long? UnitValue { get; set; }

        [BsonElement("TotalValue")]
        public long? TotalValue { get; set; }

        [BsonElement("Iva")]
        public long? Iva { get; set; }

        [BsonElement("TotalBilledValue")]
        public long? TotalBilledValue { get; set; }
    }

    // ================== BENEFICIARIOS ==================
    public class BeneficiaryNode
    {
        [BsonElement("Consecutive")]
        public int? Consecutive { get; set; }

        [BsonElement("BeneficiaryType")]
        public CodeValue? BeneficiaryType { get; set; }

        [BsonElement("IdType")]
        public CodeValue? IdType { get; set; }

        [BsonElement("IdNumber")]
        public string IdNumber { get; set; } = string.Empty;

        [BsonElement("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("MiddleName")]
        public string MiddleName { get; set; } = string.Empty;

        [BsonElement("FirstLastName")]
        public string FirstLastName { get; set; } = string.Empty;

        [BsonElement("SecondLastName")]
        public string SecondLastName { get; set; } = string.Empty;

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("CellPhoneNumber")]
        public string CellPhoneNumber { get; set; } = string.Empty;

        [BsonElement("Email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("ResidenceInfo")]
        public ResidenceInfoNode? ResidenceInfo { get; set; }

        [BsonElement("VictimRelationship")]
        public CodeValue? VictimRelationship { get; set; }

        [BsonElement("VictimIdNumber")]
        public string VictimIdNumber { get; set; } = string.Empty;

        [BsonElement("BankAccountInfo")]
        public BankAccountInfoNode? BankAccountInfo { get; set; }

        [BsonElement("LegalRepresentativeInfo")]
        public List<LegalRepresentativeNode>? LegalRepresentativeInfo { get; set; }
    }

    public class BeneficiaryLiquidationNode : BeneficiaryNode
    {
        [BsonElement("Gender")]
        public CodeValue? Gender { get; set; }

        [BsonElement("BeneficiaryStatus")]
        public string BeneficiaryStatus { get; set; } = string.Empty;

        [BsonElement("ApplyPayment")]
        public bool? ApplyPayment { get; set; }

        [BsonElement("ApprovedPaymentValue")]
        public long? ApprovedPaymentValue { get; set; }

        [BsonElement("ApprovedPaymentPercentage")]
        public long? ApprovedPaymentPercentage { get; set; }

        [BsonElement("ObjectionValue")]
        public long? ObjectionValue { get; set; }

        [BsonElement("ObjectionPercentage")]
        public long? ObjectionPercentage { get; set; }

        [BsonElement("TransactionInfo")]
        public BeneficiaryTransactionInfoNode? TransactionInfo { get; set; }

        [BsonElement("CreationUser")]
        public string CreationUser { get; set; } = string.Empty;

        [BsonElement("CreationDate")]
        public DateTime? CreationDate { get; set; }
    }

    public class BeneficiaryTransactionInfoNode
    {
        [BsonElement("Type")]
        public CodeValue? Type { get; set; }

        [BsonElement("TransactionId")]
        public string TransactionId { get; set; } = string.Empty;

        [BsonElement("BankAccountInfo")]
        public BankAccountInfoNode? BankAccountInfo { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("MiddleName")]
        public string MiddleName { get; set; } = string.Empty;

        [BsonElement("FirstLastName")]
        public string FirstLastName { get; set; } = string.Empty;

        [BsonElement("SecondLastName")]
        public string SecondLastName { get; set; } = string.Empty;
    }

    // ================== REMISIÓN / TRANSPORTE ==================
    public class RemissionInfoNode
    {
        [BsonElement("ReferenceType")]
        public CodeValue? ReferenceType { get; set; }

        [BsonElement("RemissionDate")]
        public DateTime? RemissionDate { get; set; }

        [BsonElement("ExitHour")]
        public string ExitHour { get; set; } = string.Empty;

        [BsonElement("ReferringHealthCareProviderAuthorizationCode")]
        public string ReferringHealthCareProviderAuthorizationCode { get; set; } = string.Empty;

        [BsonElement("ProfessionalSender")]
        public string ProfessionalSender { get; set; } = string.Empty;

        [BsonElement("ProfessionalSenderPosition")]
        public string ProfessionalSenderPosition { get; set; } = string.Empty;

        [BsonElement("AcceptationDate")]
        public string AcceptationDate { get; set; } = string.Empty;

        [BsonElement("AcceptationHour")]
        public string AcceptationHour { get; set; } = string.Empty;

        [BsonElement("ReceivingHealthCareProviderAuthorizationCode")]
        public string ReceivingHealthCareProviderAuthorizationCode { get; set; } = string.Empty;

        [BsonElement("ProfessionalReceptor")]
        public string ProfessionalReceptor { get; set; } = string.Empty;

        [BsonElement("Transport")]
        public TransportNode? Transport { get; set; }
    }

    public class TransportNode
    {
        [BsonElement("PrimaryTransferAmbulancePlate")]
        public string PrimaryTransferAmbulancePlate { get; set; } = string.Empty;

        [BsonElement("InterinstitutionalTransferAmbulancePlate")]
        public string InterinstitutionalTransferAmbulancePlate { get; set; } = string.Empty;

        [BsonElement("VictimTransportFromEventSite")]
        public string VictimTransportFromEventSite { get; set; } = string.Empty;

        [BsonElement("VictimTransportUntilJournyEnd")]
        public string VictimTransportUntilJournyEnd { get; set; } = string.Empty;

        [BsonElement("TransportServiceType")]
        public CodeValue? TransportServiceType { get; set; }

        [BsonElement("VictimPickedUpZone")]
        public CodeValue? VictimPickedUpZone { get; set; }
    }

    // ================== PAGOS / TOTALES ==================
    public class PaymentDetailsNode
    {
        [BsonElement("ConsolidatedPaymentsValue")]
        public long? ConsolidatedPaymentsValue { get; set; }

        [BsonElement("ConsolidatedAcceptancesValue")]
        public long? ConsolidatedAcceptancesValue { get; set; }

        [BsonElement("PaymentValue")]
        public long? PaymentValue { get; set; }

        [BsonElement("PaymentDate")]
        public DateTime? PaymentDate { get; set; }

        [BsonElement("PaymentRequestDate")]
        public DateTime? PaymentRequestDate { get; set; }

        [BsonElement("PaymentAuthorizationNumber")]
        public string PaymentAuthorizationNumber { get; set; } = string.Empty;

        [BsonElement("PaymentOrderNumber")]
        public string PaymentOrderNumber { get; set; } = string.Empty;

        [BsonElement("ClaimFormalizationDate")]
        public DateTime? ClaimFormalizationDate { get; set; }
    }

    public class TotalGlossValuesNode
    {
        [BsonElement("TotalInvoiceClaimedValue")]
        public long? TotalInvoiceClaimedValue { get; set; }

        [BsonElement("TotalInvoiceApprovedValue")]
        public long? TotalInvoiceApprovedValue { get; set; }

        [BsonElement("InvoiceAceptedValue")]
        public long? InvoiceAcceptedValue { get; set; }

        [BsonElement("TotalInvoiceObjectedValue")]
        public long? TotalInvoiceObjectedValue { get; set; }

        [BsonElement("ApprovalType")]
        public string ApprovalType { get; set; } = string.Empty;
    }

    public class ObservationNode
    {
        [BsonElement("Consecutive")]
        public int? Consecutive { get; set; }

        [BsonElement("Observation")]
        public string Observation { get; set; } = string.Empty;

        [BsonElement("UserRole")]
        public string UserRole { get; set; } = string.Empty;

        [BsonElement("UserName")]
        public string UserName { get; set; } = string.Empty;

        [BsonElement("CreationDate")]
        public DateTime? CreationDate { get; set; }
    }

    // ================== ALERTAS / WORKFLOW / FLAGS ==================
    public class AlertNode
    {
        [BsonElement("NameAction")]
        public string NameAction { get; set; } = string.Empty;

        [BsonElement("Type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("Module")]
        public string Module { get; set; } = string.Empty;

        [BsonElement("Message")]
        public string Message { get; set; } = string.Empty;

        [BsonElement("Description")]
        public string Description { get; set; } = string.Empty;
    }

    public class WorkflowDataNode
    {
        [BsonElement("Status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("Identifier")]
        public string Identifier { get; set; } = string.Empty;

        [BsonElement("Detail")]
        public string Detail { get; set; } = string.Empty;

        [BsonElement("InitiationDate")]
        public DateTime? InitiationDate { get; set; }
    }

    public class AuditProcessFlagsNode
    {
        [BsonElement("DentistAudit")]
        public SimpleAuditNode? DentistAudit { get; set; }

        [BsonElement("InstrumentationAudit")]
        public SimpleAuditNode? InstrumentationAudit { get; set; }

        [BsonElement("MedicalAudit")]
        public SimpleAuditNode? MedicalAudit { get; set; }

        [BsonElement("TariffAudit")]
        public SimpleAuditNode? TariffAudit { get; set; }
    }

    public class SimpleAuditNode
    {
        [BsonElement("ApplyAudit")]
        public bool? ApplyAudit { get; set; }

        [BsonElement("Processed")]
        public bool? Processed { get; set; }
    }

    // ================== EXTENSIONES ==================
    //public static class SolidariaInvoiceDataExtensions
    //{
    //    public static string GetAccidentNumber(this SolidariaInvoiceData d)
    //        => d.Sections.InvolvedVehicleInformation.LicensePlate
    //           ?? string.Empty;

    //    public static string GetPrimaryAmbulancePlate(this SolidariaInvoiceData d)
    //        => d.Claim?.Victims?.Find(v => v.RemissionInfo?.Transport?.PrimaryTransferAmbulancePlate != null)
    //               ?.RemissionInfo?.Transport?.PrimaryTransferAmbulancePlate
    //           ?? string.Empty;

    //    public static string? GetVehiclePlate(this SolidariaInvoiceData d)
    //        => d.Sections.InvolvedVehicleInformation.LicensePlate;
    //    public static string? GetSoatPolicyNumber(this SolidariaInvoiceData d)
    //        => d.Sections.InvolvedVehicleInformation.SoatNumber;

    //    public static string? GetFirstVictimId(this SolidariaInvoiceData d)
    //        => d.Claim?.Victims != null && d.Claim.Victims.Count > 0
    //           ? d.Claim.Victims[0].IdNumber
    //           : null;

    //    public static DateTime? GetEventDate(this SolidariaInvoiceData d)
    //        => d.Sections.EventInformation.EventDate; 

    //    public static string? ToUTCString(this DateTime? dt, string format)
    //        => dt.HasValue ? dt.Value.ToUniversalTime().ToString(format) : null;
    //}
}