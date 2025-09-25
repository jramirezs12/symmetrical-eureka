using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RulesEngine.Domain.Primitives;
using System.Text.Json;

namespace RulesEngine.Domain.Invoices.Entities
{
    [BsonIgnoreExtraElements]
    public class Invoice : Entity<string>
    {
        public string InvoiceOrigin { get; private set; } = string.Empty;
        public IEnumerable<VirtualProcess> VirtualProcesses { get; private set; } = Enumerable.Empty<VirtualProcess>();
        public string RadNumber { get; private set; } = string.Empty;
        public string? MainRadNumber { get; set; } = string.Empty;
        public string? LastRadNumber { get; private set; } = string.Empty;
        public string LoteIQ { get; private set; } = string.Empty;
        public string? LastLoteIQ { get; private set; } = string.Empty;
        public string TypeIdentIps { get; private set; } = string.Empty;
        public string NitIps { get; private set; } = string.Empty;
        public string NameIps { get; private set; } = string.Empty;
        public string InvoiceNumber { get; private set; } = string.Empty;
        public string HelpType { get; private set; } = string.Empty;
        public string PathFileAzure { get; private set; } = string.Empty;
        public string SucursalId { get; private set; } = string.Empty;
        public string SucursalCityCode { get; private set; } = string.Empty;
        public string SucursalMunicipality { get; private set; } = string.Empty;
        public string SucursalCity { get; private set; } = string.Empty;
        public string SucursalAddress { get; private set; } = string.Empty;
        public string SucursalPhone { get; private set; } = string.Empty;
        public IEnumerable<Email> SucursalEmails { get; private set; } = Enumerable.Empty<Email>();
        public bool RipsV1 { get; private set; } = false;
        public bool RipsV2 { get; private set; } = false;
        public bool Furips { get; private set; } = false;
        public bool HasMixedCoverage { get; private set; }
        public Dictionary<string, object> Detail { get; private set; }
        public DateTime DateReception { get; private set; } = DateTime.MinValue;
        public DateTime DateRadication { get; private set; } = DateTime.MinValue;
        public DateTime DateIntegration { get; private set; } = DateTime.MinValue;
        public DateTime DateCreation { get; private set; } = DateTime.MinValue;

        [BsonElement("WorkflowData")]
        public BsonDocument WorkflowData { get; private set; }
        [BsonElement("ProcessFlowData")]
        public BsonDocument ProcessFlowData { get; private set; }
        public bool IsReconciled { get; set; }
        public bool IsSigned { get; set; }
        public bool HasIpsConciliation { get; set; }
        public bool HasAuditConciliation { get; set; }
        [BsonElement("TotalSummaryData")]
        public BsonDocument TotalSummaryData { get; set; }
        public BsonDocument ResearchResponse { get; set; }
        public bool IsInternal { get; set; }
        public int ClaimantType { get; set; }
        public int ClaimantTypeId { get; set; }
        public CommonType? ProcessLine { get; set; }
        public List<Atypicality> Atypicalities { get; set; }


        private Invoice(string Id,
                        string radNumber,
                        string invoiceNumber,
                        string nitIps,
                        Dictionary<string, object> detail) : base(Id)
        {
            RadNumber = radNumber;
            InvoiceNumber = invoiceNumber;
            NitIps = nitIps;
            Detail = detail;
        }

        private Invoice(string Id,
                        string invoiceOrigin,
                        string radNumber,
                        string loteIQ,
                        string typeIdentIps,
                        string nitIps,
                        string nameIps,
                        string invoiceNumber,
                        string helpType,
                        string pathFileAzure,
                        string sucursalId,
                        string sucursalCityCode,
                        string sucursalMunicipality,
                        string sucursalCity,
                        string sucursalAddress,
                        string sucursalPhone,
                        IEnumerable<Email> sucursalEmails,
                        bool ripsV1,
                        bool ripsV2,
                        bool furips,
                        Dictionary<string, object> detail,
                        DateTime dateReception,
                        DateTime dateRadication,
                        DateTime dateIntegration,
                        DateTime dateCreation,
                        BsonDocument workflowData,
                        BsonDocument processFlowData,
                        bool hasMixedCoverage,
                        bool isReconciled,
                        bool isSigned,
                        bool hasIpsConciliation,
                        bool hasAuditConciliation,
                        BsonDocument totalSummaryData) : base(Id)
        {
            InvoiceOrigin = invoiceOrigin;
            RadNumber = radNumber;
            LoteIQ = loteIQ;
            TypeIdentIps = typeIdentIps;
            NitIps = nitIps;
            NameIps = nameIps;
            InvoiceNumber = invoiceNumber;
            HelpType = helpType;
            PathFileAzure = pathFileAzure;
            SucursalId = sucursalId;
            SucursalCityCode = sucursalCityCode;
            SucursalMunicipality = sucursalMunicipality;
            SucursalCity = sucursalCity;
            SucursalAddress = sucursalAddress;
            SucursalPhone = sucursalPhone;
            SucursalEmails = sucursalEmails;
            RipsV1 = ripsV1;
            RipsV2 = ripsV2;
            Furips = furips;
            Detail = detail;
            DateReception = dateReception;
            DateRadication = dateRadication;
            DateIntegration = dateIntegration;
            DateCreation = dateCreation;
            WorkflowData = workflowData;
            ProcessFlowData = processFlowData;
            HasMixedCoverage = hasMixedCoverage;
            IsReconciled = isReconciled;
            IsSigned = isSigned;
            HasIpsConciliation = hasIpsConciliation;
            TotalSummaryData = totalSummaryData;
        }


        public static Invoice Create(string radNumber,
                                     string invoiceNumber,
                                     string nitIps,
                                     Dictionary<string, object> detail)
        {
            var Id = new BsonObjectId(ObjectId.GenerateNewId());
            return new Invoice(Id.ToString(), radNumber, invoiceNumber, nitIps, detail);
        }

        public static Invoice Create(string invoiceOrigin, string radNumber,
                                     string loteIQ, string typeIdentIps,
                                     string nitIps, string nameIps,
                                     string invoiceNumber, string helpType,
                                     string pathFileAzure, string sucursalId,
                                     string sucursalCityCode, string sucursalMunicipality,
                                     string sucursalCity, string sucursalAddress,
                                     string sucursalPhone, IEnumerable<Email> sucursalEmails,
                                     bool ripsV1, bool ripsV2, bool furips,
                                     Dictionary<string, object> detail,
                                     DateTime dateReception, DateTime dateRadication,
                                     DateTime dateIntegration, DateTime dateCreation,
                                     BsonDocument workflowData, BsonDocument processFlowData,
                                     bool hasMixedCoverage, bool isReconciled,
                                     bool isSigned, bool hasIpsConciliation,
                                     bool hasAuditConciliation, BsonDocument totalSummaryData)
        {
            var _id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();
            return new Invoice(_id, invoiceOrigin, radNumber, loteIQ, typeIdentIps, nitIps, nameIps, invoiceNumber, helpType, pathFileAzure,
                               sucursalId, sucursalCityCode, sucursalMunicipality, sucursalCity, sucursalAddress, sucursalPhone, sucursalEmails,
                               ripsV1, ripsV2, furips, detail, dateReception, dateRadication, dateIntegration, dateCreation, workflowData, processFlowData,
                               hasMixedCoverage, isReconciled, isSigned, hasIpsConciliation, hasAuditConciliation, totalSummaryData);
        }

        public static Invoice Update(string Id, string invoiceOrigin, string radNumber,
                                     string loteIQ, string typeIdentIps,
                                     string nitIps, string nameIps,
                                     string invoiceNumber, string helpType,
                                     string pathFileAzure, string sucursalId,
                                     string sucursalCityCode, string sucursalMunicipality,
                                     string sucursalCity, string sucursalAddress,
                                     string sucursalPhone, IEnumerable<Email> sucursalEmails,
                                     bool ripsV1, bool ripsV2, bool furips,
                                     Dictionary<string, object> detail,
                                     DateTime dateReception, DateTime dateRadication,
                                     DateTime dateIntegration, DateTime dateCreation,
                                     BsonDocument workflowData, BsonDocument processFlowData,
                                     bool hasMixedCoverage, bool isReconciled,
                                     bool isSigned, bool hasIpsConciliation, bool hasAuditConciliation,
                                     BsonDocument totalSummaryData)
        {
            return new Invoice(Id, invoiceOrigin, radNumber, loteIQ, typeIdentIps, nitIps, nameIps, invoiceNumber, helpType, pathFileAzure,
                               sucursalId, sucursalCityCode, sucursalMunicipality, sucursalCity, sucursalAddress, sucursalPhone, sucursalEmails,
                               ripsV1, ripsV2, furips, detail, dateReception, dateRadication, dateIntegration, dateCreation, workflowData, processFlowData,
                               hasMixedCoverage, isReconciled, isSigned, hasIpsConciliation, hasAuditConciliation, totalSummaryData);
        }

        private static BsonDocument CreateBsonDocument(Dictionary<string, JsonElement> dinamicos)
        {
            var bsonDocument = new BsonDocument();

            foreach (var kvp in dinamicos)
            {
                bsonDocument.Add(kvp.Key, ConvertirJsonElement(kvp.Value));
            }

            return bsonDocument;
        }

        private static BsonValue ConvertirJsonElement(JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Array:
                    var bsonArray = new BsonArray();
                    foreach (var item in jsonElement.EnumerateArray())
                    {
                        bsonArray.Add(ConvertirJsonElement(item));
                    }
                    return bsonArray;

                case JsonValueKind.Object:
                    var bsonDocument = new BsonDocument();
                    foreach (var prop in jsonElement.EnumerateObject())
                    {
                        bsonDocument.Add(prop.Name, ConvertirJsonElement(prop.Value));
                    }
                    return bsonDocument;
                case JsonValueKind.String:
                    return BsonValue.Create(jsonElement.GetString());
                case JsonValueKind.Number:
                    return BsonValue.Create(jsonElement.GetDecimal());
            }

            return BsonNull.Value;
        }

        public class Email
        {
            public string BasicEmail { get; set; } = string.Empty;
            public bool? State { get; set; } = null;
            public DateTime? NotificationDateCertimail { get; set; } = null;
            public string NotificationType { get; set; } = string.Empty;
            public IEnumerable<string> EmailsCc { get; set; } = Enumerable.Empty<string>();
            public IEnumerable<string> EmailsCco { get; set; } = Enumerable.Empty<string>();
        }

        public class VirtualProcess
        {
            public string VirtualID { get; set; } = string.Empty;
            public DateTime Date { get; set; }

            public VirtualProcess(string virtualID, DateTime date)
            {
                VirtualID = virtualID;
                Date = date;
            }

            public static VirtualProcess Create(string virtualID, DateTime date)
            {
                return new VirtualProcess(virtualID, date);
            }
        }
    }
}