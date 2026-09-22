using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Notes.Entities
{
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;
        public string Contenu { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }

        public int IdPatient { get; set; }
    }
}
