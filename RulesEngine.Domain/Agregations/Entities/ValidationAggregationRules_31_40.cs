namespace RulesEngine.Domain.Agregations.Entities
{
    public class ValidationAggregationRules_31_40
    {
        public List<ResultValdiation> LicensePlateCase { get; set; }
        public List<ResultValdiation> SoatNumberCase { get; set; }
        public List<ResultValdiation> IdentificationNumberCase { get; set; }
        public int ParameterRule35And36 { get; set; }
        public int ParameterRule37And38 { get; set; }
        public int ParameterRule39And40 { get; set; }
    }

    public class ResultValdiation
    {
        public string RadNumber { get; set; }
        public string LicensePlate { get; set; }
        public string SoatNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string EventDate { get; set; }
        public string DocumentType { get; set; }
        public string VehicleType { get; set; }
    }
}
