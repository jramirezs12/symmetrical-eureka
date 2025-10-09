using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RulesEngine.Domain.Invoices.Entities
{
    /// <summary>
    /// Tipo común parametrizable. Ajustado para aceptar subdocumentos que incluyan
    /// un campo `_Id` (con mayúscula) y potencialmente otros campos adicionales.
    /// </summary>
    [BsonIgnoreExtraElements] // Ignora cualquier campo adicional no mapeado
    public class CommonType
    {
        /// <summary>
        /// Identificador interno en algunas colecciones (viene como `_Id`).
        /// Se deja nullable para tolerar ausencia.
        /// </summary>
        [BsonElement("_Id")]
        [BsonRepresentation(BsonType.Int32)] // Si a veces viene como string convertible
        public int? InternalId { get; set; }

        [BsonElement("Code")]
        public string Code { get; set; } = string.Empty;

        [BsonElement("Description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("State")]
        [BsonDefaultValue(false)]
        public bool State { get; set; }

        /// <summary>
        /// Captura de campos no mapeados por si deseas inspeccionarlos (opcional).
        /// </summary>
        [BsonExtraElements]
        public IDictionary<string, object>? ExtraElements { get; set; }
    }
}