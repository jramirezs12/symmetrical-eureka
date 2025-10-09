using MongoDB.Bson.Serialization.Attributes;
using RulesEngine.Domain.Invoices.Entities;
using RulesEngine.Domain.Primitives;

namespace RulesEngine.Domain.ClaimsQueue.Entities
{
    /// <summary>
    /// Entidad de cola de reclamaciones. Marcada para ignorar elementos extra
    /// y permitir deserialización robusta de ProcessLine.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ClaimsQueueEntity : Entity<string>
    {
        public string RadNumber { get; private set; } = string.Empty;
        public string? UserAccount { get; set; } = string.Empty;
        public string WorkflowID { get; private set; } = string.Empty;
        public string? SucursalId { get; set; }
        public Module? Module { get; private set; }
        public ClaimStatus? ClaimStatus { get; private set; }

        // Si en la colección está exactamente como "ProcessLine".
        // Si en Mongo está "processLine" cambia a [BsonElement("processLine")]
        [BsonElement("ProcessLine")]
        public CommonType? ProcessLine { get; private set; } = null;

        public Role? Role { get; private set; }
        public DateTime? RadicationDate { get; private set; } = null;
        public DateTime CreationDate { get; private set; } = DateTime.MinValue;
        public DateTime? AssignmentDate { get; set; } = null;
        public DateTime? CompletionDate { get; private set; } = null;
        public bool? IsInternal { get; private set; }
    }
}