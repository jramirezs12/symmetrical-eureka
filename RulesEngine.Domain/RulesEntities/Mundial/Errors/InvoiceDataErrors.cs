using ErrorOr;
using System.Text.RegularExpressions;

namespace RulesEngine.Domain.RulesEntities.Mundial.Errors
{
    public class InvoiceDataErrors
    {
        public static Error ValidationErros { get; } = Error.Validation(
           description: "Se han generado varios errores de validación.");
        public static Error PreviousRadNumberLength { get; } = Error.Validation(
          description: "El número de radicado anterior debe ser de máximo 10 carácteres.");
        public static Error PreviousRadNumberEmpty { get; } = Error.Validation(
          description: "El número de radicado anterior es requerido.");
        public static Error ClaimConsecutiveLength { get; } = Error.Validation(
            description: "El consecutivo de reclamación debe ser de máximo 12 carácteres.");
        public static Error ClaimConsecutiveEmpty { get; } = Error.Validation(
            description: "El consecutivo de reclamación es requerido");
        public static Error HabilitationCodeProviderLength { get; } = Error.Validation(
            description: "El código de habilitación del prestador debe der de maximo 12 carácteres");
        public static Error HabilitationCodeProviderEmpty { get; } = Error.Validation(
            description: "El código de habilitación del prestador es requerido");
        public static Error VictimFirstNameLength { get; } = Error.Validation(
            description: "El nombre de la victima debe de ser maximo 20 carácteres");
        public static Error VictimFirstNameEmpty { get; } = Error.Validation(
            description: "El nombre de la victima es requerido");
        public static Error VictimLastNameLength { get; } = Error.Validation(
            description: "El apellido de la victima debe de ser maximo 20 carácteres");
        public static Error VictimLastNameEmpty { get; } = Error.Validation(
            description: "El apellido de la victima es requerido");
        public static Error VictimDocumentTypeLength { get; } = Error.Validation(
            description: "El tipo de documento de la victima debe ser de maximo 2 carácteres");
        public static Error DocumentTypeFormat { get; } = Error.Validation(
           description: "El tipo de documento debe ser: CC, CE, CN, PA, TI, RC, AS, MS, CD, SC, PE, PT, DE");
        public static Error VictimDocumentTypeEmpty { get; } = Error.Validation(
            description: "El tipo de documento de la victima de la victima es requerido");
        public static Error VictimIdLength { get; } = Error.Validation(
            description: "El documento de identidad de la victima debe ser de maximo 16 carácteres");
        public static Error VictimIdEmpty { get; } = Error.Validation(
            description: "El documento de identidad de la victima es requerido");
        public static Error VictimGenreLength { get; } = Error.Validation(
            description: "El genero de la victima debe de ser maximo 1 carácter");
        public static Error VictimGenreEmpty { get; } = Error.Validation(
            description: "El genero de la victima es requerido");
        public static Error VictimGenreType { get; } = Error.Validation(
            description: "El valor especificado no es valido, debe de se: F, M, O");
        public static Error VictimResidentAddressLength { get; } = Error.Validation(
            description: "La dirección de residencia de la victima debe ser de maximo 100 carácteres");
        public static Error VictimResidentAddressEmpty { get; } = Error.Validation(
            description: "La direccion de residencia de la victima es obligatoria");
        public static Error DepartmentResidentVictimCodeLength { get; } = Error.Validation(
            description: "El código del departamento de residencia victima debe ser maximo de 2 carácteres");
        public static Error DepartmentResidentVictimEmpty { get; } = Error.Validation(
            description: "El código del departamento de residencia de la victima es requerido");
        public static Error MunicipalitytResidentVictimCodeLength { get; } = Error.Validation(
            description: "El código del municipio de residencia debe ser maximo de 2 carácteres");
        public static Error MunicipalityResidentVictimEmpty { get; } = Error.Validation(
            description: "El código del municipio de residencia de la victima es requerido");
        public static Error VictimTelephoneNumberLength { get; } = Error.Validation(
            description: "El número de telefono de la victima debe ser de maximo 10 carácteres");
        public static Error VictimTelephoneNumberEmpty { get; } = Error.Validation(
            description: "El número de telefono de la victima es requerido");
        public static Error VictimTypeConditionLength { get; } = Error.Validation(
            description: "El tipo de condición de la victima debe ser de maximo 1 carácter");
        public static Error VictimTypeConditionEmpty { get; } = Error.Validation(
            description: "El tipo de condición de la victima es ogligatorio");
        public static Error VictimTypeCondition { get; } = Error.Validation(
            description: "El tipo de condición de la victima debe de ser: 1, 2, 3 o 4");
        public static Error NatureOfEventType { get; } = Error.Validation(
            description: "El tipo de naturaleza del evento debe de ser: 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14, 15, 16, 17, 25, 26, 27.");
        public static Error NatureOfEventLength { get; } = Error.Validation(
            description: "El número de naturaleza del evento debe de ser maximo 2 carácteres");
        public static Error NatureOfEventEmpty { get; } = Error.Validation(
            description: "El código de naturaleza del evento es obligatoria");
        public static Error EventDescriptionLength { get; } = Error.Validation(
            description: "La descripción del evento de debe de ser maximo de 25 carácteres");
        public static Error EventDescriptionEmpty { get; } = Error.Validation(
            description: "La descripción del evento es obligatoria");
        public static Error EventAddressLength { get; } = Error.Validation(
            description: "La dirección del evento debe de ser maximo de 100 carácteres");
        public static Error EventAddressEmpty { get; } = Error.Validation(
            description: "La dirección del evento es obligatoria");
        public static Error EventHourLength { get; } = Error.Validation(
            description: "La hora del evento debe de ser maximo de 5 carácteres");
        public static Error EventHourFormat { get; } = Error.Validation(
            description: "La hora del evento debe de ser en formato HH:MM (24 Horas)");
        public static Error EventHourEmpty { get; } = Error.Validation(
            description: "La hora del evento es obligatoria");
        public static Error EventDateEmpty { get; } = Error.Validation(
            description: "La fecha del evento es requerida");
        public static Error EventDateLength { get; } = Error.Validation(
            description: "La fecha del evento debe de tener maximo 10 caracteres");
        public static Error EventDateFormat { get; } = Error.Validation(
            description: "el formato de La fecha de inicio de vigencia de la poliza es invalido, debe de ser: Formato DD/MM/AAAA");
        public static Error EventDepartmentCodeLength { get; } = Error.Validation(
            description: "El código del departamento de ocurrencia del evento debe ser maximo de 2 carácteres");
        public static Error EventDepartmentCodeEmpty { get; } = Error.Validation(
            description: "El código del departamento del evento es requerido");
        public static Error EventMunicipalityCodeLength { get; } = Error.Validation(
            description: "El código del municipio de ocurrencia del evento debe ser maximo de 3 carácteres");
        public static Error EventMunicipalityCodeEmpty { get; } = Error.Validation(
            description: "El código del municipio de ocurrencia del evento es requerido");
        public static Error EventZoneOcurrenceType { get; } = Error.Validation(
            description: "La zona del evento debe de ser: U, R");
        public static Error EventZoneOcurrenceEmpty { get; } = Error.Validation(
            description: "La zona del evento es requerido");
        public static Error EventZoneOcurrenceLength { get; } = Error.Validation(
            description: "La zona del evento debe ser maximo de 1 carácter");
        public static Error EventZoneLength { get; } = Error.Validation(
            description: "La zona de ocurrencia del evento debe ser maximo de 1 carácter");
        public static Error OtherEventDescriptionEmpty { get; } = Error.Validation(
            description: "la descripción del evento es requerida");
        public static Error OtherEventDescriptionLength { get; } = Error.Validation(
            description: "la descripción del evento debede ser maximo de 42 carácteres");
        public static Error AssuranceStateType { get; } = Error.Validation(
            description: "El estado de la aseguradora debe de ser: 1, 2, 3, 4, 5, 6, 7 y 8");
        public static Error AssuranceStateLength { get; } = Error.Validation(
            description: "El estado de la aseguradora debe ser maximo de 1 carácter");
        public static Error AssuranceStateEmpty { get; } = Error.Validation(
            description: "El estado de la aseguradora es requerido");
        public static Error BrandLength { get; } = Error.Validation(
            description: "La marca debe de tener maximo 15 carácteres");
        public static Error BrandEmpty { get; } = Error.Validation(
            description: "El campo marca es requerido");
        public static Error LicensePlateLength { get; } = Error.Validation(
            description: "El número de placa debe de tener maximo 10 carácteres");
        public static Error LicensePlateEmpty { get; } = Error.Validation(
            description: "El campo número de placa marca es requerido");
        public static Error SoatNumberLength { get; } = Error.Validation(
            description: "El número poliza SOAT debe de tener maximo 20 carácteres");
        public static Error SoatNumberEmpty { get; } = Error.Validation(
            description: "El camponúmero de poliza SOAT es requerido");
        public static Error InsuranceCompanyCodeLength { get; } = Error.Validation(
            description: "El código de la aseguradora debe de tener maximo 8 carácteres");
        public static Error InsuranceCompanyCodeEmpty { get; } = Error.Validation(
            description: "El código de la aseguradora es requerido");
        public static Error VehicleType { get; } = Error.Validation(
            description: "El tipo de vehiculo debe ser: 1, 2, 3, 4, 5, 6, 7, 8,10, 14, 17, 19, 20, 21, 22");
        public static Error VehicleTypeLength { get; } = Error.Validation(
            description: "El tipo de vehiculo debe ser maximo de 1 carácter");
        public static Error VehicleTypeEmpty { get; } = Error.Validation(
            description: "El tipo de vehiculo es requerido");
        public static Error InsuranceCodeLength { get; } = Error.Validation(
            description: "El código de la seguradora debe de tener maximo 6 carácteres");
        public static Error InsuranceCodeEmpty { get; } = Error.Validation(
            description: "El codigo de la aseguradora es requerido");
        public static Error PolicyValidityStartDateEmpty { get; } = Error.Validation(
            description: "La fecha de inicio de vigencia de la poliza es requerida");
        public static Error PolicyValidityStartDateLength { get; } = Error.Validation(
            description: "La fecha de inicio de vigencia de la poliza debe de tener maximo 10 caracteres");
        public static Error PolicyValidityStartDateFormat { get; } = Error.Validation(
            description: "el formato de La fecha de inicio de vigencia de la poliza es invalido, debe de ser: Formato DD/MM/AAAA");
        public static Error PolicyValidityEndDateEmpty { get; } = Error.Validation(
            description: "La fecha de final de vigencia de la poliza es requerida");
        public static Error PolicyValidityEndDateLength { get; } = Error.Validation(
            description: "La fecha de final de vigencia de la poliza debe de tener maximo 10 caracteres");
        public static Error PolicyValidityEndDateFormat { get; } = Error.Validation(
            description: "el formato de La fecha de final de vigencia de la poliza es invalido, debe de ser: Formato DD/MM/AAAA");
        public static Error RadNumberSirasLength { get; } = Error.Validation(
            description: "El número de radicado SIRAS debe de tener maximo 20 carácteres");
        public static Error RadNumberSirasEmpty { get; } = Error.Validation(
            description: "El número de radicado SIRAS es requerido");
        public static Error InsuranceCapExhaustionChargeLength { get; } = Error.Validation(
            description: "El cobro por agotamiento debe ser maximo 1 carácter");
        public static Error InsuranceCapExhaustionChargeEmpty { get; } = Error.Validation(
            description: "El cobro por agotamiento es requerido");
        public static Error InsuranceCapExhaustionChargeType { get; } = Error.Validation(
            description: "El cobro por agotamiento es invalido, debe de ser: 0 o 1");
        public static Error ComplexityOfSurgicalProcedureLength { get; } = Error.Validation(
            description: "El código de Complejidad del procedimiento quirúrgico debe ser maximo de 1 carácter");
        public static Error ComplexityOfSurgicalProcedureType { get; } = Error.Validation(
            description: "El código de Complejidad del procedimiento quirúrgico debe ser: 1, 2 o 3");
        public static Error ComplexityOfSurgicalProcedureEmpty { get; } = Error.Validation(
            description: "El código de Complejidad del procedimiento quirúrgico es requerido");
        public static Error CupsMainProcedureLength { get; } = Error.Validation(
            description: "El código CUPS del procedimiento quirúrgico principal debe ser maximo de 6 carácteres");
        public static Error CupsMainProcedureEmpty { get; } = Error.Validation(
            description: "El código CUPS del procedimiento quirúrgico principal es requerido");
        public static Error CupsSecondProcedureLength { get; } = Error.Validation(
            description: "El código CUPS del procedimiento quirúrgico secundario debe ser maximo de 6 carácteres");
        public static Error CupsSecondProcedureEmpty { get; } = Error.Validation(
            description: "El código CUPS del procedimiento quirúrgico secundario es requerido");
        public static Error UCIServiceType { get; } = Error.Validation(
            description: "El tipo de servicio UCI debe ser: 0 o 1");
        public static Error UCIServiceLength { get; } = Error.Validation(
            description: "La prestación del servicio UCI debe ser maximo de 1 carácter");
        public static Error UCIServiceEmpty { get; } = Error.Validation(
            description: "El tipo de servicio UCi es requerido");
        public static Error ClaimDaysUciLength { get; } = Error.Validation(
            description: "Los dias reclamados  de la UCI deben ser maximo 2 caracteres");
        public static Error ClaimDaysUciEmpty { get; } = Error.Validation(
            description: "El campo días reclamados UCI es requerido");
        public static Error FirstSurnameOwnerOrCompanyNameLength { get; } = Error.Validation(
            description: "El primer apellido del propietario del vehiculo debe ser maximo de 40 carácteres");
        public static Error FirstSurnameOwnerOrCompanyNameEmpty { get; } = Error.Validation(
            description: "El primer apellido del propietario del vehiculo es requerido");
        public static Error FirstNameOwnerLength { get; } = Error.Validation(
            description: "El nombre del propietario del vehiculo debe ser maximo de 20 carácteres");
        public static Error FirstNameOwnerEmpty { get; } = Error.Validation(
            description: "El nombre del propietario del vehiculo es requerido");
        public static Error AddressResidenceOwnerLength { get; } = Error.Validation(
            description: "La dirección de residencia del propietario del vehiculo debe ser maximo de 200 carácteres");
        public static Error TelephoneNumberOwnerEmpty { get; } = Error.Validation(
            description: "El número de telefono del propietario esrequerido");
        public static Error TelephoneNumberOwnerLength { get; } = Error.Validation(
            description: "El número de telefono del propietario debe ser maximo de 10 caráteres");
        public static Error AddressResidenceOwnerEmpty { get; } = Error.Validation(
            description: "La dirección de resisdencia del propietario del vehiculo es requerida");
        public static Error DepartmentResidenceOwnerLength { get; } = Error.Validation(
            description: "El código del departamento de residencia del propietario del vehiculo debe ser maximo de 2 carácteres");
        public static Error DepartmentResidenceOwnerEmpty { get; } = Error.Validation(
            description: "El código del departamento de residencia del propietario del vehiculo es requerido");
        public static Error MunicipalityResidenceOwnerLength { get; } = Error.Validation(
            description: "El código del municipio de residencia del propietario del vehiculo debe ser maximo de 3 carácteres");
        public static Error MunicipalityResidenceOwnerEmpty { get; } = Error.Validation(
            description: "El código del municipio de residencia del pripiedario del vehiculo es requerido");
        public static Error FirstSurnameDriverLength { get; } = Error.Validation(
            description: "El primer apellido del conductor debe ser maximo de 20 carácteres");
        public static Error FirstSurnameDriverEmpty { get; } = Error.Validation(
            description: "El primer apellido del conductor es requerido");
        public static Error FirstNameDriverLength { get; } = Error.Validation(
            description: "El nombre del conductor debe ser maximo de 20 carácteres");
        public static Error FirstNameDriverEmpty { get; } = Error.Validation(
            description: "El nombre del conductor es requerido");
        public static Error DocumentTypeDriverLength { get; } = Error.Validation(
            description: "El tipo de documento del conductor debe ser maximo de 2 carácteres");
        public static Error DocumentTypeDriver { get; } = Error.Validation(
            description: "El tipo de documento del conductor debe ser: CC, CE, PA, RC, TI, MS, AS, CD, SC, DE, PE, PT");
        public static Error DocumentTypeDriverEmpty { get; } = Error.Validation(
            description: "El tipo de documento del conductor es requerido");
        public static Error DocumentNumberDriverLength { get; } = Error.Validation(
            description: "El número de documento del conductor debe ser maximo de 16 carácteres");
        public static Error DocumentNumberDriverEmpty { get; } = Error.Validation(
            description: "El número de documento del conductor es requerido");
        public static Error ResidenceAddressDriverLength { get; } = Error.Validation(
            description: "La dirección de redisencia del conductor debe ser maximo de 200 carácteres");
        public static Error ResidenceAddressDriverEmpty { get; } = Error.Validation(
            description: "La dirección de redisencia del conductor es requerido");
        public static Error DepartmentResidenceCodeDriverLength { get; } = Error.Validation(
            description: "El código del departamento de residencia del conductor debe ser maximo de 2 carácteres");
        public static Error DepartmentResidenceCodeDriverEmpty { get; } = Error.Validation(
            description: "El código del departamento de residencia del conductor debe ser maximo de 2 carácteres");
        public static Error MunicipalityResidenceCodeDriverEmpty { get; } = Error.Validation(
            description: "El código del municipio de residencia del conductor es requerido");
        public static Error MunicipalityResidenceCodeDriverLength { get; } = Error.Validation(
            description: "El código del municipio de residencia del conductor debe ser maximo de 3 carácteres");
        public static Error ThelephoneNumberDriverEmpty { get; } = Error.Validation(
            description: "El número de telefono del conductor es requerido");
        public static Error ThelephoneNumberDriverLength { get; } = Error.Validation(
            description: "El número de telefono de conductor debe ser maximo de 10 carácteres");
        public static Error ReferenceTypeLength { get; } = Error.Validation(
            description: "El tipo de referencia debe de tener maximo 1 carácter");
        public static Error ReferenceType { get; } = Error.Validation(
            description: "El tipo de referencia debe de  ser: 1, 2 o 3");
        public static Error ReferenceTypeEmpty { get; } = Error.Validation(
            description: "El tipo de referencia es requerido");
        public static Error RemisionDateLength { get; } = Error.Validation(
            description: "La fecha de remisión debe de ser maximo de 10 caracteres");
        public static Error RemisionDateFormat { get; } = Error.Validation(
            description: "El formato de la fecha de remision es invalido, debe de ser: Formato DD/MM/AAAA");
        public static Error RemisionDateEmpty { get; } = Error.Validation(
            description: "La fecha de remisión es requerida");
        public static Error DepartingTimeLength { get; } = Error.Validation(
            description: "La Hora de Salida debe ser maximo de 5 carácteres");
        public static Error DepartingTimeFormat { get; } = Error.Validation(
            description: "El formato de la Hora de Salida es invalido, debe de ser: Formato HH:MM (24 Horas)");
        public static Error DepartingTimeEmpty { get; } = Error.Validation(
            description: "La Hora de Salida es requerida");
        public static Error HabilitationCodeProviderSenderLength { get; } = Error.Validation(
            description: "El código de habilitación del prestador remitente debe ser de maximo 12 carácteres");
        public static Error HabilitationCodeProviderSenderEmpty { get; } = Error.Validation(
            description: "El código de habilitación del prestador remitente es requerido");
        public static Error ProfessionalRefersLength { get; } = Error.Validation(
            description: "El campo profesional que remite debe de ser maximo 60 carácteres");
        public static Error ProfessionalRefersEmpty { get; } = Error.Validation(
            description: "El campo profesional que remite debe de ser maximo 60 carácteres");
        public static Error PositionPersonSenderEmpty { get; } = Error.Validation(
            description: "El cargo de la persona que remite es requerido");
        public static Error PositionPersonSenderLength { get; } = Error.Validation(
            description: "El cargo de la persona que remite debe de ser maximo 30 carácteres");
        public static Error AceptationDateEmpty { get; } = Error.Validation(
            description: "La fecha de aceptación es requerida");
        public static Error AceptationDateLength { get; } = Error.Validation(
            description: "La fecha de aceptación debe de ser maximo 10 carácteres");
        public static Error AceptationDateFormat { get; } = Error.Validation(
            description: "La fecha de aceptacion es invalida, debe de ser: Formato DD/MM/AAAA");
        public static Error AceptationHourEmpty { get; } = Error.Validation(
            description: "La hora de aceptación es requerida");
        public static Error AceptationHourLength { get; } = Error.Validation(
            description: "La hora de aceptación debe de ser maximo 5 carácteres");
        public static Error AceptationHourFormat { get; } = Error.Validation(
            description: "La hora de aceptación es invalida, debe de ser Formato HH:MM (24 Horas)");
        public static Error RemissionHabilitationCodeProviderLength { get; } = Error.Validation(
            description: "El código de habilitación del prestador que recibe debe ser de maximo 12 carácteres");
        public static Error RemissionHabilitationCodeProviderEmpty { get; } = Error.Validation(
            description: "El código de habilitación del prestador que recibe es requerido");
        public static Error ProfessionalWhoReceivedEmpty { get; } = Error.Validation(
            description: "El profesional que recibe es requerido");
        public static Error ProfessionalWhoReceivedLength { get; } = Error.Validation(
            description: "El profesional que recibe debe ser de maximo 60 carácteres");
        public static Error RemissionAmbulancePlateEmpty { get; } = Error.Validation(
            description: "La placa de la ambulancia que realiza el traslado es requerida");
        public static Error RemissionAmbulancePlateLength { get; } = Error.Validation(
            description: "La placa de la ambulancia que realiza el traslado es debe ser maximo de 6 carácteres");
        public static Error PrimaryTransferAmbulancePlateEmpty { get; } = Error.Validation(
            description: "La placa de la ambulancia de traslado primario es requerido");
        public static Error PrimaryTransferAmbulancePlateLength { get; } = Error.Validation(
            description: "La placa de ambulancia de tralado primario debe de ser maximo de 6 carácteres");
        public static Error TransportationVictimEventEmpty { get; } = Error.Validation(
            description: "El transporte de la victima al evento es requerido");
        public static Error TransportationVictimEventLength { get; } = Error.Validation(
            description: "El transporte de la victima al evento debe ser de maximo 40 carácter");
        public static Error TransportationVictimEndJourneyEmpty { get; } = Error.Validation(
            description: "El campo transporte de la victima hasta el fin del recorrido es requerido");
        public static Error TransportationVictimEndJourneyLength { get; } = Error.Validation(
            description: "El campo transporte de la victima hasta el fin del recorrido debe ser de maximo de 40 carácteres");
        public static Error SertviceTransportTypeLenght { get; } = Error.Validation(
            description: "El tipo de servicio de transporte debe ser de maximo 1 carácter");
        public static Error SertviceTransportTypeEmpty { get; } = Error.Validation(
            description: "El campo Tipo servicio de transporte es requerido");
        public static Error ServiceTransportType { get; } = Error.Validation(
            description: "El tipo de servicio de transporte es invalido debe de ser: 1, 2");
        public static Error AreaVictimCollectedEmpty { get; } = Error.Validation(
            description: "El campo zona de recolección de la victima es requerido");
        public static Error AreaVictimCollectedLength { get; } = Error.Validation(
            description: "La zona de recollección de la victima debe de ser maximo de 1 carácter");
        public static Error AreaVictimCollectedType { get; } = Error.Validation(
            description: "El tipo de zona de recolección de la victima es invalido, debe de ser: U, R");
        public static Error AdmisionTimeMedicalCertificationEmpty { get; } = Error.Validation(
            description: "la hora de ingreso de certificación médica es requerido");
        public static Error MedicalCertificationIncomeDateEmpty { get; } = Error.Validation(
            description: "La Fecha de ingreso es requerida");
        public static Error MedicalCertificationIncomeDateLength { get; } = Error.Validation(
            description: "La Fecha de ingreso debe de tener maximo 10 caracteres");
        public static Error MedicalCertificationIncomeDateFormat { get; } = Error.Validation(
            description: "el formato de La Fecha de ingreso de vigencia de la poliza es invalido, debe de ser: Formato DD/MM/AAAA");
        public static Error MedicalCertificationEgressDateEmpty { get; } = Error.Validation(
            description: "La Fecha de ingreso es requerida");
        public static Error MedicalCertificationEgressDateLength { get; } = Error.Validation(
            description: "La Fecha de ingreso debe de tener maximo 10 caracteres");
        public static Error MedicalCertificationEgressDateFormat { get; } = Error.Validation(
            description: "el formato de La Fecha de ingreso de vigencia de la poliza es invalido, debe de ser: Formato DD/MM/AAAA");
        public static Error AdmisionTimeMedicalCertificationLength { get; } = Error.Validation(
            description: "la hora de ingreso debe de ser maximo de 5 carácteres");
        public static Error AdmisionTimeMedicalCertificationFormat { get; } = Error.Validation(
            description: "El formato de la hora de ingreso certificación médica es invalido, debe de ser: Formato HH:MM (24 Horas)");
        public static Error DischageTimeMedicaCertificationEmpty { get; } = Error.Validation(
            description: "La hora de egreso de certificación médica es requerida");
        public static Error DischageTimeMedicaCertificationLength { get; } = Error.Validation(
            description: "La hora de egreso de certificación médica debe ser maximo de 5 carácteres");
        public static Error DischageTimeMedicaCertificationFormat { get; } = Error.Validation(
            description: "El formato de la hora de egreso de certificación médica es invalido, debe de ser: Formato HH:MM (24 Horas)");
        public static Error MedicalCertificationDiagonsisIncomeEmpty { get; } = Error.Validation(
            description: "El código de diagnostico principal de ingreso certificación médica es requerido");
        public static Error MedicalCertificationDiagonsisIncomeCodification { get; } = Error.Validation(
            description: "El código de diagnostico principal de ingreso certificación médica no tiene la codificacion CIE-10");
        public static Error MedicalCertificationDiagonsisIncomeLength { get; } = Error.Validation(
            description: "El código de diasgnostico principal de ingreso de certificación médica debe de ser maximo de 4 carácteres");
        public static Error MedicalCertificationDiagonsisEgressEmpty { get; } = Error.Validation(
            description: "El código de diagnostico principal de egreso certificación médica es requerido");
        public static Error MedicalCertificationDiagonsisEgressLength { get; } = Error.Validation(
            description: "El código de diasgnostico principal de egreso de certificación médica debe de ser maximo de 4 carácteres");
        public static Error MedicalCertificationDiagonsisEgressCodification { get; } = Error.Validation(
            description: "El código de diagnostico principal de egreso certificación médica no tiene la codificacion CIE-10");
        public static Error FirstSurnameProfessionalHealthEmpty { get; } = Error.Validation(
            description: "El primer apellido del médico o profesional de salud es requerido");
        public static Error FirstSurnameProfessionalHealthLength { get; } = Error.Validation(
            description: "El primer apellido del médico o profesional de salud debe de ser maximo de 20 carácteres");
        public static Error FirstNameProfessionalHealthEmpty { get; } = Error.Validation(
            description: "El primer nombre del médico o profesional de salud es requerido");
        public static Error FirstNameProfessionalHealthLength { get; } = Error.Validation(
            description: "El primer nombre del médico o profesional de salud debe de ser maximo de 20 carácteres");
        public static Error DocumentTypeProfessionalHealthEmpty { get; } = Error.Validation(
            description: "El tipo de documento de identidad del médico o profesional de es requerido");
        public static Error DocumentTypeProfessionalHealthLength { get; } = Error.Validation(
            description: "El tipo de documento de identidad del médico o profesional de salud debe de ser maximo de 2 carácteres");
        public static Error DocumentTypeProfessionalHealth { get; } = Error.Validation(
            description: "El tipo de documento de identidad del médico profesional de salud es invalido, debe de ser: CC, CE, PE, PA, PT");
        public static Error IdentificationNumberProfessionalHealthEmpty { get; } = Error.Validation(
            description: "El númerode ientificación del profesional de salud es requerido");
        public static Error IdentificationNumberProfessionalHealthLength { get; } = Error.Validation(
            description: "El número de identificación del profesional de salud debe ser maximo de 16 carácteres");
        public static Error MedicalRecordNumberEmpty { get; } = Error.Validation(
            description: "El número de registro del médico es requerido");
        public static Error MedicalRecordNumberLength { get; } = Error.Validation(
            description: "El número de registro del médico debe ser maximo de 16 carácteres");
        public static Error TotalBilledMedicalAndSurgicalExpensesEmpty { get; } = Error.Validation(
            description: "El total de gastos médicos y quirúrgicos facturados es requerido");
        public static Error TotalBilledMedicalAndSurgicalExpensesLength { get; } = Error.Validation(
            description: "El total de gastos médicos y quirúrgicos facturados debe ser de maximo 15 carácteres");
        public static Error TotalClaimedMedicalAndSurgicalExpensesEmpty { get; } = Error.Validation(
            description: "El total de gastos médicos y quirúrgicos reclamados es requerido");
        public static Error TotalClaimedMedicalAndSurgicalExpensesLength { get; } = Error.Validation(
            description: "El total de gastos médicos y quirúrgicos reclamados debe ser de maximo 15 carácteres");
        public static Error ToTalBilledTransportAndMobilizationExpensesEmpty { get; } = Error.Validation(
            description: "El total de gastos de transporte y movilización facturados es requerido");
        public static Error TotalBilledTransportAndMobilizationExpensesLength { get; } = Error.Validation(
            description: "El total de gastos de transporte y movilización facturados debe ser de maximo 15 carácteres");
        public static Error ToTalClaimedTransportAndMobilizationExpensesEmpty { get; } = Error.Validation(
            description: "El total de gastos de transporte y movilización reclamados es requerido");
        public static Error ToTalClaimedTransportAndMobilizationExpensesLength { get; } = Error.Validation(
            description: "El total de gastos de transporte y movilización reclamados debe ser de maximo 15 carácteres");

    }
}
