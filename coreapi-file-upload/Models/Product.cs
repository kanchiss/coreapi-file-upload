using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace coreapi_file_upload.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("Name")]
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public string ProductImageName { get; set; } = null!;
    }
}
