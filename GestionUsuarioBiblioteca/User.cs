using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GestionUsuarioBiblioteca
{
    [BsonNoId]
    [BsonIgnoreExtraElements]
    public class User
    {
        public required int Id { get; set; }

        public required string Name { get; set; }
        public bool HasActiveLoan { get; set; }
        public bool HasPenalty { get; set; }
    }
}
