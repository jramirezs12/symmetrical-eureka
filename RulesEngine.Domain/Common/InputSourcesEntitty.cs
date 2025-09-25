using RulesEngine.Domain.InputSourcesEntities;

namespace RulesEngine.Domain.Common
{
    public abstract class InputSourcesEntitty
    {
        public FraudulentIps? FraudulentIps { get; set; }
        public IpsInvestigation? IpsInvestigation { get; set; }
        public List<AtypicalEvent>? AtypicalEvent { get; set; }
        public List<CatastrophicEvent>? CatastrophicEvent { get; set; }
        public List<DuplicatedInvoice> DuplicatedInvoices { get; set; } = [];
        public List<IpsPhoneVerification>? IpsPhoneVerification { get; set; } = [];
        public List<AmbulanceControl>? AmbulanceControl { get; set; } = [];
        public List<IpsNitFile>? IpsNitList { get; set; }
        public List<AllowedUser>? AllowedUsers { get; set; }
        public List<InvoiceNumberFile>? InvoiceNumberFile { get; set; }
        public List<ServiceCodeFile>? ServiceCodeFiles { get; set; }
        public List<SinisterDuplicity>? SinisterDuplicity { get; set; } = [];
    }
}
