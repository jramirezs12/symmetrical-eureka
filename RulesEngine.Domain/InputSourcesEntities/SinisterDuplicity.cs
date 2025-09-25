namespace RulesEngine.Domain.InputSourcesEntities
{
    public class SinisterDuplicity
    {
        /// <summary>
        /// Furisp1, involvedVehicleInformation, soatnumber =  poliza
        /// </summary>
        public required string SoatNumber { get; set; }

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
    }
}
