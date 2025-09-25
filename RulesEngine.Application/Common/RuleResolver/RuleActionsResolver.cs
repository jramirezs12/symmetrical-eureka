using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RulesEngine.Domain.Common;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Application.Common.Rule
{
    public class RuleActionsResolver
    {
        private readonly IInvoiceToCheckContext _Entity;
        private readonly IInvoiceRepository _InvoiceRepository;

        public RuleActionsResolver(IInvoiceToCheckContext entity, IInvoiceRepository invoiceRepository)
        {
            _Entity = entity;
            _InvoiceRepository = invoiceRepository;
        }

        public async Task ExecuteActions()
        {
            //Insert alert object into mongo DB
            await InsertAlert();
        }

        private async Task InsertAlert()
        {
            var filter =
                 Builders<Invoice>.Filter.Eq(x => x.RadNumber, _Entity.RadNumber) &
                 Builders<Invoice>.Filter.Eq("Detail.moduleName", _Entity.ModuleName) &
                 Builders<Invoice>.Filter.Eq(x => x.NitIps, _Entity.IpsNit);

            var bdInvoice = await _InvoiceRepository!.FindOneAsync(filter);

            if (bdInvoice != null)
            {
                List<Alert> existingAlerts = new List<Alert>();

                if (bdInvoice.Detail.ContainsKey("Alerts"))
                {
                    var alertsValue = bdInvoice.Detail["Alerts"];
                    string alertsJson;

                    if (alertsValue is string)
                        alertsJson = alertsValue.ToString()!;
                    else if (alertsValue is IEnumerable<object> alertsList)
                        alertsJson = JsonConvert.SerializeObject(alertsList);
                    else
                        alertsJson = string.Empty;

                    if (!string.IsNullOrWhiteSpace(alertsJson) && IsValidJson(alertsJson))
                        existingAlerts = JsonConvert.DeserializeObject<List<Alert>>(alertsJson) ?? new List<Alert>();
                }

                List<Alert> newAlerts = _Entity.Alerts ?? new List<Alert>();
                List<Alert> updatedAlerts = existingAlerts.Concat(newAlerts).DistinctBy(a => new {
                    a.AlertAction,
                    a.AlertNameAction,
                    a.AlertType,
                    a.AlertDescription,
                    a.AlertMessage,
                    a.Typification,
                    a.HasPriority
                }).ToList();

                bdInvoice.Detail["Alerts"] = updatedAlerts;

                var detailToJson = System.Text.Json.JsonSerializer.Serialize(bdInvoice.Detail);

                if (BsonDocument.TryParse(detailToJson, out BsonDocument result))
                {
                    var update = Builders<Invoice>.Update.Set("Detail", result);
                    await _InvoiceRepository!.UpdateOneAsync(filter, update, new UpdateOptions());
                }
            }
        }
        private bool IsValidJson(string jsonString)
        {
            jsonString = jsonString.Trim();
            if (jsonString.StartsWith("{") && jsonString.EndsWith("}") ||
                jsonString.StartsWith("[") && jsonString.EndsWith("]"))
            {
                try
                {
                    JToken.Parse(jsonString);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
