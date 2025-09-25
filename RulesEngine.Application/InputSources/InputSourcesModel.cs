using RulesEngine.Application.InputSources.FromFile;

namespace RulesEngine.Application.InputSources
{
    public class InputSourcesModel
    {
        public required FraudulentIpsFromFile FraudulentIps { get; init; }
        public required IpsInvestigationFromFile IpsInvestigation { get; init; }
        public required AtypicalEventFromFile AtypicalEvent { get; init; }
        public required CatastrophicEventFromFile CatastrophicEvent { get; init; }
        public required IpsPhoneVerificationFromFIle IpsPhoneVerification { get; init; }
        public required AmbulanceControlFromFile AmbulanceControl { get; init; }
        public required ListIpsNitFromFile IpsNitList { get; init; }
        public required UsersAllowedClaimFromFile AllowedUsers { get; init; }
        public required InvoiceNumberFromFile InvoiceNumber { get; init; }
        public required ServiceCodesFromFile ServiceCodes { get; init; }
    }
}
