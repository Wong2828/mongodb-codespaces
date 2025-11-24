using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StudentAPI.Models;

namespace StudentAPI.Services;

public class StudentService
{
    private readonly IMongoCollection<Student> _collection;

    public StudentService(IOptions<StudentDatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _collection = db.GetCollection<Student>(settings.Value.CollectionName);
    }

    public async Task<List<Student>> GetAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<Student?> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Student s) =>
        await _collection.InsertOneAsync(s);

    public async Task UpdateAsync(string id, Student updated) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, updated);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);

    public async Task CreateManyAsync(List<Student> students) =>
        await _collection.InsertManyAsync(students);

    public async Task<List<Student>> GetByLastNameAsync(string lastName) =>
        await _collection.Find(s => s.LastName == lastName).ToListAsync();

    public async Task<List<Student>> GetYoungerThanAsync(int age) =>
        await _collection.Find(s => s.Age < age).ToListAsync();
}
