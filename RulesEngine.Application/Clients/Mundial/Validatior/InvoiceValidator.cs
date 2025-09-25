using FluentValidation;
using RulesEngine.Domain.Constants;
using RulesEngine.Domain.Constants.Entities;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.RulesEntities.Mundial.Errors;
using RulesEngine.Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace RulesEngine.Application.Clients.Mundial.Validatior
{
    public class InvoiceValidator : AbstractValidator<InvoiceData>
    {
        private readonly IConstantsRepository _constantsRepository;
        private readonly Regex _validateHour = new Regex(@"^(?:[01]\d|2[0-3]):[0-5]\d$");
        private readonly Regex _validateCIE10 = new Regex(@"^[A-Z]\\d{2}(\\.\\d{1,2})?$");

        public InvoiceValidator(IConstantsRepository constantsRepository)
        {
            _constantsRepository = constantsRepository;

            #region Datos Reclamación
            RuleFor(x => x.Sections!.ClaimDataFurips1!.Consecutiveclaimnumber)
                .NotEmpty().NotNull().GreaterThanOrEqualTo(0).WithMessage("{PropertyName} " + InvoiceDataErrors.ClaimConsecutiveEmpty.Description)
                .When(x => x.Sections!.ClaimDataFurips1 != null)
                .Must(value => value != null && value.ToString()!.Length <= 12).WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.ClaimConsecutiveLength.Description)
                .WithName("Número consecutivo de la reclamación");
            #endregion

            #region Prestador servicios de salud
            #endregion

            #region Datos de la victima - evento catastrofico  o accidente de transito
            RuleFor(x => x.Sections!.VictimData!.FirstLastName)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.VictimLastNameEmpty.Description)
                .When(x => x.Sections!.VictimData != null)
                .Must(value => value != null && value.Length <= 40)
                .WithErrorCode("CustomMustValidator")
                .WithName("Primer apellido");

            RuleFor(x => x.Sections!.VictimData!.Name)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.VictimFirstNameEmpty.Description)
                .When(x => x.Sections!.VictimData != null)
                .Must(value => value != null && value.Length <= 20)
                .WithErrorCode("CustomMustValidator")
                .WithName("Primer nombre");

            RuleFor(x => x.Sections!.VictimData!.DocumentType)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.VictimDocumentTypeEmpty.Description)
                .WithName("Tipo de documento")
                .Must(value => value != null && value!.Value.Length <= 2)
                .WithErrorCode("CustomMustValidator")
                .WithMessage("{PropertyName} " + InvoiceDataErrors.VictimDocumentTypeLength.Description)
                .MustAsync(async (value, cancellation) =>
                {
                    ConstantsEntity allowedDocumentTypes = await GetAllowedDocumentTypesAsync();
                    return allowedDocumentTypes.ListType!.Exists(x => x != null && value != null && x.Description!.ToString() == value!.Name);
                }).WithMessage("{PropertyName} " + InvoiceDataErrors.DocumentTypeFormat.Description)
                .WithName("Tipo de documento");

            RuleFor(x => x.Sections!.VictimData!.IdentificationNumber)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.VictimIdEmpty.Description)
                .When(x => x.Sections!.VictimData != null)
                .Must(value => value != null && value.Length <= 16)
                .WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.VictimIdLength.Description)
                .WithName("Número de identificación");

            RuleFor(x => x.Sections!.VictimData!.AccidentCondition)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.VictimTypeConditionEmpty.Description)
                .WithName("Condición de la víctima")
                .Must(x => x != null && x!.Value.Length == 1)
                .WithErrorCode("CustomMustValidator")
                .MustAsync(async (value, cancellation) =>
                {
                    ConstantsEntity condition = await GetConditionAsync();
                    return condition.ListType!.Exists(x => x != null && value != null && x.Description!.ToString() == value!.Name);
                }).WithMessage("{PropertyName} " + InvoiceDataErrors.VictimTypeCondition.Description)
                .WithName("Condición de la víctima");

            #endregion

            #region Datos del sitio - ocurrencia del evento

            RuleFor(x => x.Sections!.CatastrophicPlaceEvent!.EventNature)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.NatureOfEventEmpty.Description)
                //.When(x => x.Sections!.CatastrophicPlaceEvent != null)
                .Must(value => value != null && value!.Value.Length <= 2)
                .WithErrorCode("CustomMustValidator")
                .MustAsync(async (value, cancellation) =>
                {
                    ConstantsEntity natureOfEvent = await GetNatureOfEventAsync();
                    return natureOfEvent.ListType!.Exists(x => x != null && value != null && x.Code == value!.Value);
                }).WithMessage("{PropertyName} " + InvoiceDataErrors.NatureOfEventType.Description)
                .WithName("Naturaleza del evento");

            RuleFor(x => x.Sections!.CatastrophicPlaceEvent!.AccidentCause)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.EventDescriptionEmpty.Description)
                .When(x => x.Sections!.CatastrophicPlaceEvent != null)
                .Must(value => value != null && value.Value.Length <= 45)
                .WithErrorCode("CustomMustValidator")
                .WithName("Causa del evento");

            RuleFor(x => x.Sections!.CatastrophicPlaceEvent!.Address)
                .NotEmpty().WithMessage("{PropertyName} " + InvoiceDataErrors.EventAddressEmpty.Description)
                .When(x => x.Sections!.CatastrophicPlaceEvent != null)
                .Must(value => value != null && value.Length <= 100)
                .WithErrorCode("CustomMustValidator")
                .WithName("Dirección de la ocurrencia");

            RuleFor(x => x.Sections!.CatastrophicPlaceEvent!.EventDate)
                .Must(x =>
                {
                    string? formattedDate = string.Empty;
                    if (x == null || x.Value == null) return false;

                    formattedDate = x.Value == null ? null : x.Value.ToString("dd/MM/yyyy");
                    if (string.IsNullOrEmpty(formattedDate))
                        return false;
                    var eventDate = Date.Create(formattedDate!);

                    if (Date.IsNullable(eventDate))
                        return false;

                    if (Date.IsFailedConversion(eventDate))
                        return false;

                    return formattedDate.Length == 10;
                }).WithErrorCode("CustomMustValidator")
                .WithMessage(c =>
                {
                    if (c.Sections?.CatastrophicPlaceEvent ?.EventDate == null)
                        return InvoiceDataErrors.EventDateEmpty.Description;

                    string formattedDate = c.Sections?.CatastrophicPlaceEvent?.EventDate.Value.ToString("dd/MM/yyyy")!;
                    var remissionDate = Date.Create(formattedDate);

                    if (Date.IsNullable(remissionDate))
                        return InvoiceDataErrors.EventDateEmpty.Description;

                    if (Date.IsFailedConversion(remissionDate))
                        return InvoiceDataErrors.EventDateFormat.Description;

                    if (remissionDate.Value.ToString()!.Length <= 10)
                        return InvoiceDataErrors.EventDateLength.Description;

                    return InvoiceDataErrors.RemisionDateFormat.Description;
                })//.When(x => x.Sections!.RemissionDate != null)
                .WithName("Fecha de ocurrencia del evento"); ;



            RuleFor(x => x.Sections!.CatastrophicPlaceEvent!.EventHour)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.EventHourEmpty.Description)
                .When(x => x.Sections!.CatastrophicPlaceEvent != null)
                .Must(x=> x != null && x.Length <= 5)
                //.WithErrorCode("CustomMustValidator")
                //.Matches(_validateHour).WithMessage(InvoiceDataErrors.EventHourFormat.Description)
                .WithName("Hora de ocurrencia del evento");

            RuleFor(x => x.Sections!.CatastrophicPlaceEvent!.EventDepartment)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.EventDepartmentCodeEmpty.Description)
                .WithName("Nombre departamento ocurrencia")
                .When(x => x.Sections!.CatastrophicPlaceEvent != null)
                .Must(x => x != null && x!.Value.Length <= 45)
                .WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.EventDepartmentCodeLength.Description)
                .WithName("Nombre departamento ocurrencia");

            RuleFor(x => x.Sections!.CatastrophicPlaceEvent!.EventMunicipality)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.EventMunicipalityCodeEmpty.Description)
                .When(x => x.Sections!.CatastrophicPlaceEvent != null)
                .Must(x => x != null && x!.Value.Length <= 42)
                .WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.EventMunicipalityCodeLength.Description)
                .WithName("Nombre municipio  ocurrencia");

            RuleFor(x => x.Sections!.CatastrophicPlaceEvent!.EventZone)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.EventZoneOcurrenceEmpty.Description)
                //.When(x => x.Sections!.CatastrophicPlaceEvent != null)
                .WithName("Zona de ocurrencia del evento")
                .Must(x=> x != null && x!.Value.Length == 1)
                .WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.EventZoneOcurrenceLength.Description)
                .MustAsync(async (value, cancellation) =>
                {
                    ConstantsEntity zone = await GetZoneAsync();
                    return zone.ListType!.Exists(x => x != null && value != null && x.Description == value!.Name);
                }).WithMessage("{PropertyName} " + InvoiceDataErrors.EventZoneOcurrenceType.Description)
                .WithName("Zona de ocurrencia del evento");

            RuleFor(x => x.Sections!.CatastrophicPlaceEvent!.OtherEvent)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.OtherEventDescriptionEmpty.Description)
                .When(x => x.Sections!.CatastrophicPlaceEvent != null)
                .MaximumLength(1000).WithMessage("{PropertyName} " + InvoiceDataErrors.OtherEventDescriptionLength.Description)
                .WithName("Descripción del evento");

            #endregion

            #region Información del vehiculo involucrado
            RuleFor(x => x.Sections!.InvolvedVehicleInformation!.SecureState)
               .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.AssuranceStateEmpty.Description)
               //.When(x => x.Sections!.InvolvedVehicleInformation != null)
               .Must(z => z != null && z!.Value.Length == 1)
               .WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.AssuranceStateLength.Description)
               .MustAsync(async (value, cancellation) =>
               {
                   ConstantsEntity assuranceState = await GetAssuranceStateAsync();
                   return assuranceState.ListType!.Exists(x => x != null && value != null && x.Code == value!.Value);
               }).WithMessage("{PropertyName} " + InvoiceDataErrors.AssuranceStateType.Description)
               .WithName("Estado de aseguramiento");

            RuleFor(x => x.Sections!.InvolvedVehicleInformation!.VehicleMake)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.BrandEmpty.Description)
                .When(x => x.Sections!.InvolvedVehicleInformation != null)
                .MaximumLength(15).WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.BrandLength.Description)
                .WithName("Marca vehículo");

            RuleFor(x=>x.Sections!.InvolvedVehicleInformation!.VehicleType)
                .NotNull().NotEmpty().WithMessage("{PropertyName} " + InvoiceDataErrors.VehicleTypeEmpty.Description)
                //.When(x => x.Sections!.InvolvedVehicleInformation != null)
                .Must(x=> x != null && x!.Value.Length <= 1)
                .WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.VehicleTypeLength.Description)
                .MustAsync(async (value, cancellation) =>
                {
                    ConstantsEntity vehicleType = await GetVehicleTypeAsync();
                    return vehicleType.ListType!.Exists(x => x != null && value !=null && x.Code == value!.Value);
                }).WithMessage("{PropertyName} " + InvoiceDataErrors.VehicleType.Description)
                .WithName("Tipo de Vehículo");

            RuleFor(x => x.Sections!.InvolvedVehicleInformation!.LicensePlate)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.LicensePlateEmpty.Description)
                .When(x => x.Sections!.InvolvedVehicleInformation != null)
                .MaximumLength(10).WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.LicensePlateLength.Description)
                .WithName("Placa");

            RuleFor(x => x.Sections!.InvolvedVehicleInformation!.SoatNumber)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.SoatNumberEmpty.Description)
                .When(x => x.Sections!.InvolvedVehicleInformation != null)
                .MaximumLength(10).WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.SoatNumberLength.Description)
                .WithName("Número de póliza SOAT");

            RuleFor(x => x.Sections!.InvolvedVehicleInformation!.InsuranceCompanyCode)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.InsuranceCompanyCodeEmpty.Description)
                .When(x => x.Sections!.InvolvedVehicleInformation != null)
                .MaximumLength(8).WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.InsuranceCompanyCodeLength.Description)
                .WithName("Código de la aseguradora");

            RuleFor(x => x.Sections!.InvolvedVehicleInformation)
                .Must(x =>
                {
                    string formattedDate = string.Empty;
                    if (x == null) return true;
                    else
                    {
                        formattedDate = x.InitDateSoat == null ? null!: x.InitDateSoat.Value.ToString("dd/MM/yyyy");

                        var initDate = Date.Create(formattedDate!);
                        if (string.IsNullOrEmpty(formattedDate))
                            return true;
                        if (Date.IsNullable(initDate))
                            return false;
                        if (Date.IsFailedConversion(initDate))
                            return false;
                    }
                    

                    return formattedDate.Length == 10;
                }).WithErrorCode("CustomMustValidator")
                .WithMessage(c =>
                {
                    var formattedDate = c.Sections!.InvolvedVehicleInformation!.InitDateSoat!.Value.ToString("dd/MM/yyyy");
                    var initDate = Date.Create(formattedDate);

                    if (Date.IsNullable(initDate))
                        return InvoiceDataErrors.PolicyValidityStartDateEmpty.Description;

                    if (Date.IsFailedConversion(initDate))
                        return InvoiceDataErrors.PolicyValidityStartDateFormat.Description;

                    return InvoiceDataErrors.PolicyValidityStartDateFormat.Description;
                })//.When(x => x.Sections!.InvolvedVehicleInformation != null)
                .WithName("Fecha de inicio de vigencia de la póliza");

            RuleFor(x => x.Sections!.InvolvedVehicleInformation)
                .Must(x =>
                {
                    if (x == null) return true;

                    string? formattedDate = string.Empty;
                    formattedDate = x.EndDateSoat == null ? null : x.EndDateSoat!.Value.ToString("dd/MM/yyyy");
                    if (string.IsNullOrEmpty(formattedDate))
                        return true;
                    var endDate = Date.Create(formattedDate);

                    if (Date.IsNullable(endDate))
                        return false;

                    if (Date.IsFailedConversion(endDate))
                        return false;

                    return formattedDate.Length == 10;
                }).WithErrorCode("CustomMustValidator")
                .WithMessage(c =>
                {
                    var formattedDate = c.Sections!.InvolvedVehicleInformation!.EndDateSoat!.Value.ToString("dd/MM/yyyy");
                    var endDate = Date.Create(formattedDate);

                    if (Date.IsNullable(endDate))
                        return InvoiceDataErrors.PolicyValidityEndDateEmpty.Description;

                    if (Date.IsFailedConversion(endDate))
                        return InvoiceDataErrors.PolicyValidityEndDateFormat.Description;

                    return InvoiceDataErrors.PolicyValidityEndDateFormat.Description;
                })//.When(x => x.Sections!.InvolvedVehicleInformation != null)
                .WithName("Fecha final de vigencia de la póliza");
            #endregion

            #region Certificacion de la atención médica
            RuleFor(x => x.Sections!.MedicalCertification)
                .Must(x =>
                {
                    if (x == null) return true;

                    string? formattedDate = string.Empty;
                    formattedDate = x.MedicalCertificationIncomeDate == null ? null : x.MedicalCertificationIncomeDate!.Value.ToString("dd/MM/yyyy");
                    if (string.IsNullOrEmpty(formattedDate))
                        return true;
                    var incomeDate = Date.Create(formattedDate);

                    if (Date.IsNullable(incomeDate))
                        return false;

                    if (Date.IsFailedConversion(incomeDate))
                        return false;

                    return formattedDate.Length == 10;
                }).WithErrorCode("CustomMustValidator")
                .WithMessage(c =>
                {
                    var formattedDate = c.Sections!.MedicalCertification!.MedicalCertificationIncomeDate!.Value.ToString("dd/MM/yyyy");
                    var incomeDate = Date.Create(formattedDate);

                    if (Date.IsNullable(incomeDate))
                        return InvoiceDataErrors.MedicalCertificationIncomeDateEmpty.Description;

                    if (Date.IsFailedConversion(incomeDate))
                        return InvoiceDataErrors.MedicalCertificationIncomeDateFormat.Description;

                    return InvoiceDataErrors.MedicalCertificationIncomeDateFormat.Description;
                })//.When(x => x.Sections!.MedicalCertification != null)
                .WithName("Fecha de ingreso");

            RuleFor(x => x.Sections!.MedicalCertification)
                .Must(x =>
                {
                    if (x == null) return true;

                    string? formattedDate = string.Empty;
                    formattedDate = x.MedicalCertificationEgressDate == null ? null : x.MedicalCertificationEgressDate!.Value.ToString("dd/MM/yyyy");
                    if (string.IsNullOrEmpty(formattedDate))
                        return true;
                    var egressDate = Date.Create(formattedDate);

                    if (Date.IsNullable(egressDate))
                        return false;

                    if (Date.IsFailedConversion(egressDate))
                        return false;

                    return formattedDate.Length == 10;
                }).WithErrorCode("CustomMustValidator")
                .WithMessage(c =>
                {
                    var formattedDate = c.Sections!.MedicalCertification!.MedicalCertificationEgressDate!.Value.ToString("dd/MM/yyyy");
                    var egressDate = Date.Create(formattedDate);

                    if (Date.IsNullable(egressDate))
                        return InvoiceDataErrors.MedicalCertificationEgressDateEmpty.Description;

                    if (Date.IsFailedConversion(egressDate))
                        return InvoiceDataErrors.MedicalCertificationEgressDateFormat.Description;

                    return InvoiceDataErrors.MedicalCertificationEgressDateFormat.Description;
                })//.When(x => x.Sections!.MedicalCertification != null)
                .WithName("Fecha de egreso");
            
            RuleFor(x => x.Sections!.MedicalCertification!.MedicalCertificationDiagnosisEgress)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.MedicalCertificationDiagonsisEgressEmpty.Description)
                .When(x => x.Sections!.MedicalCertification != null)
                .Must(x => x != null && x.Value != null && x.Value.Length <= 4)
                .WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.MedicalCertificationDiagonsisEgressLength.Description)
                .WithName("Código  diagnóstico principal de egreso");
            #endregion

            #region Amparos que reclama

            RuleFor(x => x!.Sections!.CoveragesClaimed!.TotalBilledMedicalExpenses)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.TotalBilledMedicalAndSurgicalExpensesEmpty.Description)
                .When(x => x.Sections!.CoveragesClaimed != null)
                .Must(x => x != null && x.Value != null && x.Value >= 0)
                .WithErrorCode("CustomMustValidator")
                .Must(x => x != null && x!.Value.ToString().Length <= 15).WithMessage("{PropertyName} " + InvoiceDataErrors.TotalBilledMedicalAndSurgicalExpensesLength.Description)
                .WithName("Total facturado por amparo de gastos médicos quirúrgicos");

            RuleFor(x => x.Sections!.CoveragesClaimed!.TotalClaimMedicalExpenses)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.TotalClaimedMedicalAndSurgicalExpensesEmpty.Description)
                .When(x => x.Sections!.CoveragesClaimed != null)
                .Must(x => x != null && x!.Value >= 0)
                .WithErrorCode("CustomMustValidator")
                .Must(x => x != null && x!.Value.ToString().Length <= 15).WithMessage("{PropertyName} " + InvoiceDataErrors.TotalClaimedMedicalAndSurgicalExpensesLength.Description)
                .WithName("Total reclamado por amparo de gastos médicos quirúrgicos");

            RuleFor(x => x.Sections!.CoveragesClaimed!.TotalBilledTransportation)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.ToTalBilledTransportAndMobilizationExpensesEmpty.Description)
                .When(x => x.Sections!.CoveragesClaimed != null)
                .Must(x => x != null && x!.Value >= 0).WithErrorCode("CustomMustValidator")
                .Must(x => x != null && x!.Value.ToString().Length <= 15)
                .WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.TotalBilledTransportAndMobilizationExpensesLength.Description)
                .WithName("Total facturado por amparo de gastos de transporte y movilización de la víctima");

            RuleFor(x => x.Sections!.CoveragesClaimed!.TotalClaimTransportation)
                .NotEmpty().NotNull().WithMessage("{PropertyName} " + InvoiceDataErrors.ToTalClaimedTransportAndMobilizationExpensesEmpty.Description)
                .When(x => x.Sections!.CoveragesClaimed != null)
                .Must(x => x != null && x!.Value >= 0).WithErrorCode("CustomMustValidator")
                .Must(x => x != null && x!.Value.ToString().Length <= 15)
                .WithErrorCode("CustomMustValidator").WithMessage("{PropertyName} " + InvoiceDataErrors.ToTalClaimedTransportAndMobilizationExpensesLength.Description)
                .WithName("Total reclamado por amparo de gastos de transporte y movilización de la víctima");
            #endregion
        }

        private async Task<ConstantsEntity> GetGlossResponseReject()
        {
            return await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0111");
        }
        private async Task<ConstantsEntity> GetAllowedDocumentTypesAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0112");
            return result;
        }
        private async Task<ConstantsEntity> GetNatureOfEventAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0113");
            return result;
        }
        private async Task<ConstantsEntity> GetGenreAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0114");
            return result;
        }
        private async Task<ConstantsEntity> GetConditionAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0115");
            return result;
        }
        private async Task<ConstantsEntity> GetZoneAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0116");
            return result;
        }
        private async Task<ConstantsEntity> GetAssuranceStateAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0117");
            return result;
        }
        private async Task<ConstantsEntity> GetVehicleTypeAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0118");
            return result;
        }
        private async Task<ConstantsEntity> GetInsuranceTypeExhaustionAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0119");
            return result;
        }
        private async Task<ConstantsEntity> GetComplexityProcedureComplexityAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0120");
            return result;
        }
        private async Task<ConstantsEntity> GetHadUCIServiceAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0121");
            return result;
        }
        private async Task<ConstantsEntity> GetReferenceTypeAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0122");
            return result;
        }
        private async Task<ConstantsEntity> GetTransportServiceTypeAsync()
        {
            ConstantsEntity result = await _constantsRepository.FindOneAsync(s => s.BusinessCode == "0123");
            return result;
        }
        

    }


}
