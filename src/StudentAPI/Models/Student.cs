using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentAPI.Models;

public class Student
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName  { get; set; } = null!;
    public int Age { get; set; }
}
