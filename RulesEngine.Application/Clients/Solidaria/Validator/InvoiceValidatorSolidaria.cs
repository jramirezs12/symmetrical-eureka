using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using RulesEngine.Domain.Constants;
using RulesEngine.Domain.Constants.Entities;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.RulesEntities.Mundial.Errors;
using RulesEngine.Domain.ValueObjects;

namespace RulesEngine.Application.Clients.Solidaria.Validator
{
    public class InvoiceValidatorSolidaria : AbstractValidator<SolidariaInvoiceData>
    {
        private readonly IConstantsRepository _constantsRepository;
        private readonly Regex _validateHour = new(@"^(?:[01]\d|2[0-3]):[0-5]\d$");

        public InvoiceValidatorSolidaria(IConstantsRepository constantsRepository)
        {
            _constantsRepository = constantsRepository;
            CascadeMode = CascadeMode.Stop;

            // 1. CLAIM
            RuleFor(x => x.Claim)
                .NotNull().WithMessage(InvoiceDataErrors.ClaimConsecutiveEmpty.Description)
                .DependentRules(() =>
                {
                    RuleFor(x => x.Claim!)
                        .SetValidator(new ClaimNodeValidator(_constantsRepository));

                    // 2. EVENT (solo si existe)
                    RuleFor(x => x.Claim!.Event)
                        .NotNull().WithMessage(InvoiceDataErrors.NatureOfEventEmpty.Description)
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.Claim!.Event!)
                                .SetValidator(new EventNodeValidator(_constantsRepository, _validateHour));
                        });

                    // 3. VEHICLE
                    RuleFor(x => x.Claim!.Vehicle)
                        .NotNull().WithMessage(InvoiceDataErrors.VehicleTypeEmpty.Description)
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.Claim!.Vehicle!)
                                .SetValidator(new VehicleNodeValidator(_constantsRepository));
                        });

                    // 4. VICTIMS LIST
                    RuleFor(x => x.Claim!.Victims)
                        .NotNull().WithMessage("Debe existir al menos una víctima")
                        .Must(v => v != null && v.Count > 0)
                        .WithMessage("Debe existir al menos una víctima")
                        .DependentRules(() =>
                        {
                            // Primera víctima (si quieres todas: RuleForEach)
                            RuleFor(x => x.Claim!.Victims![0])
                                .SetValidator(new VictimNodeValidator(_constantsRepository))
                                .When(x => x.Claim!.Victims != null && x.Claim.Victims.Count > 0);
                        });
                });
        }

        // =================== VALIDADORES HIJOS ===================

        private class ClaimNodeValidator : AbstractValidator<ClaimNode>
        {
            public ClaimNodeValidator(IConstantsRepository repo)
            {
                CascadeMode = CascadeMode.Stop;

                RuleFor(c => c.Consecutive)
                    .NotNull().WithMessage(InvoiceDataErrors.ClaimConsecutiveEmpty.Description)
                    .GreaterThanOrEqualTo(0).WithMessage(InvoiceDataErrors.ClaimConsecutiveEmpty.Description)
                    .Must(v => v != null && v.ToString()!.Length <= 12)
                    .WithErrorCode("CustomMustValidator")
                    .WithMessage(InvoiceDataErrors.ClaimConsecutiveLength.Description)
                    .WithName("Número consecutivo de la reclamación");
            }
        }

        private class EventNodeValidator : AbstractValidator<EventNode>
        {
            private readonly IConstantsRepository _repo;
            public EventNodeValidator(IConstantsRepository repo, Regex hourRegex)
            {
                _repo = repo;
                CascadeMode = CascadeMode.Stop;

                RuleFor(e => e.Nature)
                    .NotNull().WithMessage(InvoiceDataErrors.NatureOfEventEmpty.Description)
                    .Must(n => n != null && n.Code != null && n.Code.Length <= 2)
                    .WithErrorCode("CustomMustValidator")
                    .MustAsync(async (n, _) =>
                    {
                        if (n == null) return false;
                        var list = await Get(ConstantsCodes.NatureOfEvent);
                        return list.ListType!.Any(c => c.Code == n.Code);
                    }).WithMessage(InvoiceDataErrors.NatureOfEventType.Description);

                RuleFor(e => e.Cause)
                    .NotNull().WithMessage(InvoiceDataErrors.EventDescriptionEmpty.Description)
                    .Must(c => c != null && c.Value.Length <= 45)
                    .WithErrorCode("CustomMustValidator");

                RuleFor(e => e.Address)
                    .NotEmpty().WithMessage(InvoiceDataErrors.EventAddressEmpty.Description)
                    .Must(a => a != null && a.Length <= 100)
                    .WithErrorCode("CustomMustValidator");

                RuleFor(e => e.Date)
                    .NotNull().WithMessage(InvoiceDataErrors.EventDateEmpty.Description)
                    .Must(ValidDate)
                    .WithErrorCode("CustomMustValidator")
                    .WithMessage(e =>
                    {
                        if (e.Date == null) return InvoiceDataErrors.EventDateEmpty.Description;
                        var formatted = e.Date.Value.ToString("dd/MM/yyyy");
                        var parsed = Date.Create(formatted);
                        if (Date.IsNullable(parsed)) return InvoiceDataErrors.EventDateEmpty.Description;
                        if (Date.IsFailedConversion(parsed)) return InvoiceDataErrors.EventDateFormat.Description;
                        if (formatted.Length != 10) return InvoiceDataErrors.EventDateLength.Description;
                        return InvoiceDataErrors.RemisionDateFormat.Description;
                    });

                RuleFor(e => e.Hour)
                    .NotEmpty().WithMessage(InvoiceDataErrors.EventHourEmpty.Description)
                    .Must(h => h != null && h.Length <= 5)
                    //.Matches(hourRegex).WithMessage(InvoiceDataErrors.EventHourFormat.Description)
                    .WithErrorCode("CustomMustValidator");

                RuleFor(e => e.Department)
                    .NotNull().WithMessage(InvoiceDataErrors.EventDepartmentCodeEmpty.Description)
                    .Must(d => d != null && d.Value.Length <= 45)
                    .WithErrorCode("CustomMustValidator")
                    .WithMessage(InvoiceDataErrors.EventDepartmentCodeLength.Description);

                RuleFor(e => e.Municipality)
                    .NotNull().WithMessage(InvoiceDataErrors.EventMunicipalityCodeEmpty.Description)
                    .Must(m => m != null && m.Value.Length <= 42)
                    .WithErrorCode("CustomMustValidator")
                    .WithMessage(InvoiceDataErrors.EventMunicipalityCodeLength.Description);

                RuleFor(e => e.Zone)
                    .NotNull().WithMessage(InvoiceDataErrors.EventZoneOcurrenceEmpty.Description)
                    .Must(z => z != null && z.Code != null && z.Code.Length == 1)
                    .WithErrorCode("CustomMustValidator")
                    .MustAsync(async (z, _) =>
                    {
                        if (z == null) return false;
                        var list = await Get(ConstantsCodes.EventZone);
                        return list.ListType!.Any(c => c.Description == z.Value || c.Code == z.Code);
                    }).WithMessage(InvoiceDataErrors.EventZoneOcurrenceType.Description);

                RuleFor(e => e.Description)
                    .NotEmpty().WithMessage(InvoiceDataErrors.OtherEventDescriptionEmpty.Description)
                    .MaximumLength(1000).WithMessage(InvoiceDataErrors.OtherEventDescriptionLength.Description);

                bool ValidDate(DateTime? d)
                {
                    if (d == null) return false;
                    var formatted = d.Value.ToString("dd/MM/yyyy");
                    var parsed = Date.Create(formatted);
                    if (Date.IsNullable(parsed)) return false;
                    if (Date.IsFailedConversion(parsed)) return false;
                    return formatted.Length == 10;
                }

                Task<ConstantsEntity> Get(string code) => _repo.FindOneAsync(c => c.BusinessCode == code);
            }
        }

        private class VehicleNodeValidator : AbstractValidator<VehicleNode>
        {
            private readonly IConstantsRepository _repo;
            public VehicleNodeValidator(IConstantsRepository repo)
            {
                _repo = repo;
                CascadeMode = CascadeMode.Stop;

                RuleFor(v => v.InsuranceStatus)
                    .NotNull().WithMessage(InvoiceDataErrors.AssuranceStateEmpty.Description)
                    .Must(s => s != null && s.Code != null && s.Code.Length == 1)
                    .WithErrorCode("CustomMustValidator").WithMessage(InvoiceDataErrors.AssuranceStateLength.Description)
                    .MustAsync(async (s, _) =>
                    {
                        if (s == null) return false;
                        var list = await Get(ConstantsCodes.AssuranceState);
                        return list.ListType!.Any(c => c.Code == s.Code);
                    }).WithMessage(InvoiceDataErrors.AssuranceStateType.Description);

                RuleFor(v => v.Brand)
                    .NotEmpty().WithMessage(InvoiceDataErrors.BrandEmpty.Description)
                    .MaximumLength(15).WithErrorCode("CustomMustValidator").WithMessage(InvoiceDataErrors.BrandLength.Description);

                RuleFor(v => v.Type)
                    .NotNull().WithMessage(InvoiceDataErrors.VehicleTypeEmpty.Description)
                    .Must(t => t != null && t.Code != null && t.Code.Length <= 1)
                    .WithErrorCode("CustomMustValidator").WithMessage(InvoiceDataErrors.VehicleTypeLength.Description)
                    .MustAsync(async (t, _) =>
                    {
                        if (t == null) return false;
                        var list = await Get(ConstantsCodes.VehicleType);
                        return list.ListType!.Any(c => c.Code == t.Code);
                    }).WithMessage(InvoiceDataErrors.VehicleType.Description);

                RuleFor(v => v.PlateNumber)
                    .NotEmpty().WithMessage(InvoiceDataErrors.LicensePlateEmpty.Description)
                    .MaximumLength(10).WithErrorCode("CustomMustValidator").WithMessage(InvoiceDataErrors.LicensePlateLength.Description);

                RuleFor(v => v.Soat!.Policy!.Number)
                    .NotEmpty().WithMessage(InvoiceDataErrors.SoatNumberEmpty.Description)
                    .When(v => v.Soat?.Policy != null)
                    .MaximumLength(10).WithErrorCode("CustomMustValidator").WithMessage(InvoiceDataErrors.SoatNumberLength.Description);

                RuleFor(v => v.Soat!.InsuranceCompany!.Code)
                    .NotEmpty().WithMessage(InvoiceDataErrors.InsuranceCompanyCodeEmpty.Description)
                    .When(v => v.Soat?.InsuranceCompany != null)
                    .MaximumLength(8).WithErrorCode("CustomMustValidator").WithMessage(InvoiceDataErrors.InsuranceCompanyCodeLength.Description);

                // Policy Validity Start
                RuleFor(v => v.Soat!.Policy)
                    .Must(StartValid)
                    .When(v => v.Soat?.Policy?.ValidityStartDate != null)
                    .WithErrorCode("CustomMustValidator")
                    .WithMessage(v =>
                    {
                        var dt = v.Soat?.Policy?.ValidityStartDate;
                        if (dt == null) return InvoiceDataErrors.PolicyValidityStartDateEmpty.Description;
                        var formatted = dt.Value.ToString("dd/MM/yyyy");
                        var parsed = Date.Create(formatted);
                        if (Date.IsNullable(parsed)) return InvoiceDataErrors.PolicyValidityStartDateEmpty.Description;
                        if (Date.IsFailedConversion(parsed)) return InvoiceDataErrors.PolicyValidityStartDateFormat.Description;
                        return InvoiceDataErrors.PolicyValidityStartDateFormat.Description;
                    });

                // Policy Validity End
                RuleFor(v => v.Soat!.Policy)
                    .Must(EndValid)
                    .When(v => v.Soat?.Policy?.ValidityEndDate != null)
                    .WithErrorCode("CustomMustValidator")
                    .WithMessage(v =>
                    {
                        var dt = v.Soat?.Policy?.ValidityEndDate;
                        if (dt == null) return InvoiceDataErrors.PolicyValidityEndDateEmpty.Description;
                        var formatted = dt.Value.ToString("dd/MM/yyyy");
                        var parsed = Date.Create(formatted);
                        if (Date.IsNullable(parsed)) return InvoiceDataErrors.PolicyValidityEndDateEmpty.Description;
                        if (Date.IsFailedConversion(parsed)) return InvoiceDataErrors.PolicyValidityEndDateFormat.Description;
                        return InvoiceDataErrors.PolicyValidityEndDateFormat.Description;
                    });

                bool StartValid(PolicyNode? p)
                {
                    var d = p?.ValidityStartDate;
                    if (d == null) return true;
                    var formatted = d.Value.ToString("dd/MM/yyyy");
                    var parsed = Date.Create(formatted);
                    if (string.IsNullOrEmpty(formatted)) return true;
                    if (Date.IsNullable(parsed)) return false;
                    if (Date.IsFailedConversion(parsed)) return false;
                    return formatted.Length == 10;
                }

                bool EndValid(PolicyNode? p)
                {
                    var d = p?.ValidityEndDate;
                    if (d == null) return true;
                    var formatted = d.Value.ToString("dd/MM/yyyy");
                    var parsed = Date.Create(formatted);
                    if (string.IsNullOrEmpty(formatted)) return true;
                    if (Date.IsNullable(parsed)) return false;
                    if (Date.IsFailedConversion(parsed)) return false;
                    return formatted.Length == 10;
                }

                Task<ConstantsEntity> Get(string code) => _repo.FindOneAsync(c => c.BusinessCode == code);
            }
        }

        private class VictimNodeValidator : AbstractValidator<VictimNode>
        {
            private readonly IConstantsRepository _repo;
            public VictimNodeValidator(IConstantsRepository repo)
            {
                _repo = repo;
                CascadeMode = CascadeMode.Stop;

                RuleFor(v => v.FirstLastName)
                    .NotEmpty().WithMessage(InvoiceDataErrors.VictimLastNameEmpty.Description)
                    .Must(s => s != null && s.Length <= 40)
                    .WithErrorCode("CustomMustValidator").WithMessage(InvoiceDataErrors.VictimLastNameEmpty.Description);

                RuleFor(v => v.FirstName)
                    .NotEmpty().WithMessage(InvoiceDataErrors.VictimFirstNameEmpty.Description)
                    .Must(s => s != null && s.Length <= 20)
                    .WithErrorCode("CustomMustValidator").WithMessage(InvoiceDataErrors.VictimFirstNameEmpty.Description);

                RuleFor(v => v.IdType)
                    .NotNull().WithMessage(InvoiceDataErrors.VictimDocumentTypeEmpty.Description)
                    .Must(t => t != null && t.Code != null && t.Code.Length <= 2)
                    .WithErrorCode("CustomMustValidator").WithMessage(InvoiceDataErrors.VictimDocumentTypeLength.Description)
                    .MustAsync(async (t, _) =>
                    {
                        if (t == null) return false;
                        var list = await Get(ConstantsCodes.DocumentType);
                        return list.ListType!.Any(c => c.Description == t.Value || c.Code == t.Code);
                    })
                    .WithMessage(InvoiceDataErrors.DocumentTypeFormat.Description);

                RuleFor(v => v.IdNumber)
                    .NotEmpty().WithMessage(InvoiceDataErrors.VictimIdEmpty.Description)
                    .Must(s => s != null && s.Length <= 16)
                    .WithErrorCode("CustomMustValidator").WithMessage(InvoiceDataErrors.VictimIdLength.Description);

                RuleFor(v => v.Condition)
                    .NotNull().WithMessage(InvoiceDataErrors.VictimTypeConditionEmpty.Description)
                    .Must(c => c != null && c.Code != null && c.Code.Length == 1)
                    .WithErrorCode("CustomMustValidator")
                    .MustAsync(async (c, _) =>
                    {
                        if (c == null) return false;
                        var list = await Get(ConstantsCodes.Condition);
                        return list.ListType!.Any(x => x.Description == c.Value || x.Code == c.Code);
                    }).WithMessage(InvoiceDataErrors.VictimTypeCondition.Description);

                // Protections (billed / claimed) — ejemplo: MedicalSurgicalExpenses
                RuleFor(v => v.ProtectionsClaimed!.MedicalSurgicalExpenses!.TotalBilled)
                    .NotNull().WithMessage(InvoiceDataErrors.TotalBilledMedicalAndSurgicalExpensesEmpty.Description)
                    .When(v => v.ProtectionsClaimed?.MedicalSurgicalExpenses != null)
                    .Must(val => val != null && val >= 0)
                    .WithErrorCode("CustomMustValidator")
                    .Must(val => val != null && val.Value.ToString().Length <= 15)
                    .WithMessage(InvoiceDataErrors.TotalBilledMedicalAndSurgicalExpensesLength.Description);

                RuleFor(v => v.ProtectionsClaimed!.MedicalSurgicalExpenses!.TotalClaimed)
                    .NotNull().WithMessage(InvoiceDataErrors.TotalClaimedMedicalAndSurgicalExpensesEmpty.Description)
                    .When(v => v.ProtectionsClaimed?.MedicalSurgicalExpenses != null)
                    .Must(val => val != null && val >= 0)
                    .WithErrorCode("CustomMustValidator")
                    .Must(val => val != null && val.Value.ToString().Length <= 15)
                    .WithMessage(InvoiceDataErrors.TotalClaimedMedicalAndSurgicalExpensesLength.Description);

                // Transporte
                RuleFor(v => v.ProtectionsClaimed!.VictimTransportAndMobilizationExpenses!.TotalBilled)
                    .NotNull().WithMessage(InvoiceDataErrors.ToTalBilledTransportAndMobilizationExpensesEmpty.Description)
                    .When(v => v.ProtectionsClaimed?.VictimTransportAndMobilizationExpenses != null)
                    .Must(val => val != null && val >= 0)
                    .WithErrorCode("CustomMustValidator")
                    .Must(val => val != null && val.Value.ToString().Length <= 15)
                    .WithMessage(InvoiceDataErrors.TotalBilledTransportAndMobilizationExpensesLength.Description);

                RuleFor(v => v.ProtectionsClaimed!.VictimTransportAndMobilizationExpenses!.TotalClaimed)
                    .NotNull().WithMessage(InvoiceDataErrors.ToTalClaimedTransportAndMobilizationExpensesEmpty.Description)
                    .When(v => v.ProtectionsClaimed?.VictimTransportAndMobilizationExpenses != null)
                    .Must(val => val != null && val >= 0)
                    .WithErrorCode("CustomMustValidator")
                    .Must(val => val != null && val.Value.ToString().Length <= 15)
                    .WithMessage(InvoiceDataErrors.ToTalClaimedTransportAndMobilizationExpensesLength.Description);

                // MedicalAttention: fechas y diagnóstico egreso
                RuleFor(v => v.MedicalAttention)
                    .Must(m => ValidDate(m?.IncomeDate))
                    .When(v => v.MedicalAttention?.IncomeDate != null)
                    .WithErrorCode("CustomMustValidator")
                    .WithMessage(v =>
                    {
                        var d = v.MedicalAttention?.IncomeDate;
                        if (d == null) return InvoiceDataErrors.MedicalCertificationIncomeDateEmpty.Description;
                        return InvoiceDataErrors.MedicalCertificationIncomeDateFormat.Description;
                    });

                RuleFor(v => v.MedicalAttention)
                    .Must(m => ValidDate(m?.EgressDate))
                    .When(v => v.MedicalAttention?.EgressDate != null)
                    .WithErrorCode("CustomMustValidator")
                    .WithMessage(v =>
                    {
                        var d = v.MedicalAttention?.EgressDate;
                        if (d == null) return InvoiceDataErrors.MedicalCertificationEgressDateEmpty.Description;
                        return InvoiceDataErrors.MedicalCertificationEgressDateFormat.Description;
                    });

                RuleFor(v => v.MedicalAttention!.EgressMainDiagnosis)
                    .NotNull().WithMessage(InvoiceDataErrors.MedicalCertificationDiagonsisEgressEmpty.Description)
                    .When(v => v.MedicalAttention != null)
                    .Must(code => code != null && code.Code != null && code.Code.Length <= 4)
                    .WithErrorCode("CustomMustValidator")
                    .WithMessage(InvoiceDataErrors.MedicalCertificationDiagonsisEgressLength.Description);

                bool ValidDate(DateTime? d)
                {
                    if (d == null) return false;
                    var formatted = d.Value.ToString("dd/MM/yyyy");
                    var result = Date.Create(formatted);
                    if (Date.IsNullable(result)) return false;
                    if (Date.IsFailedConversion(result)) return false;
                    return formatted.Length == 10;
                }

                Task<ConstantsEntity> Get(string code) => _repo.FindOneAsync(c => c.BusinessCode == code);
            }
        }

        // =================== CÓDIGOS DE CATÁLOGO (constantes) ===================
        private static class ConstantsCodes
        {
            public const string DocumentType = "0112";
            public const string NatureOfEvent = "0113";
            public const string Condition = "0115";
            public const string EventZone = "0116";
            public const string AssuranceState = "0117";
            public const string VehicleType = "0118";
        }
    }
}