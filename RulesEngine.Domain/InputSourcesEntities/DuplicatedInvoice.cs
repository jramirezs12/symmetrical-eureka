namespace RulesEngine.Domain.InputSourcesEntities
{
    public class DuplicatedInvoice
    {
        /// <summary>
        /// Furisp1, involvedVehicleInformation, soatnumber =  poliza
        /// </summary>
        public required string SoatNumber { get; set; }

        /// <summary>
        /// Furisp1, involvedVehicleInformation, licenseplate = placa
        /// </summary>
        public required string LicensePlate { get; set; }

        /// <summary>
        /// Furisp1, victimData, identificationnumber = número de documento de identidad
        /// </summary>
        public required string VictimId { get; set; }

        /// <summary>
        /// Furisp1, victimData, DocumentType, value = tipo de documento
        /// </summary>
        public required string DocumentType { get; set; }

        /// <summary>
        /// Furisp1, catastrophicPlaceEvent, EventDate = Fecha de ocurrencia del evento
        /// </summary>
        public DateTime? EventDate { get; set; }

        /// <summary>
        /// Furisp1, involvedVehicleInformation, vehicletype, value = tipo vehículo
        /// </summary>
        public string VehicleType { get; set; } = string.Empty;
        /// <summary>
        /// LicencePlateAmbulance - Evalua la placa de la ambulancia
        /// </summary>
        
        public string? LicencePlateAmbulance { get; set; } = string.Empty;
        /// <summary>
        /// HabilitationCodeProvider - Valida el códigode habilitación del prestador
        /// </summary>
        public string? HabilitationCodeProvider { get; set; } = string.Empty;
    }
}

