using ErrorOr;

namespace RulesEngine.Application.Invoices.UpdateInvoice
{
    public class UpdateInvoiceResult
    {
        public int StatusCode { get; set; }
        public int RulesExecuted { get; set; }
        public List<string>? RuleNamesMatched { get; set; }

        public UpdateInvoiceResult(int statusCode = 0, int rules = 0, List<string> ruleNamesMatched = null)
        {
            StatusCode = statusCode;
            RulesExecuted = rules;
            RuleNamesMatched = ruleNamesMatched;
        }
        
    }
}
