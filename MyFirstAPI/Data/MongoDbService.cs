using MongoDB.Driver;
using MyFirstAPI.Models;
using Microsoft.Extensions.Options;

namespace MyFirstAPI.Data
{
    public class MongoDbService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public MongoDbService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDb:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDb:DatabaseName"]);
            _usersCollection = database.GetCollection<User>(config["MongoDb:CollectionName"]);
        }

        public async Task<List<User>> GetUsersAsync() => await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User> GetUserByIdAsync(int id) => await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task AddUserAsync(User user) => await _usersCollection.InsertOneAsync(user);

        public async Task UpdateUserAsync(User user) => await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);

        public async Task DeleteUserAsync(int id) => await _usersCollection.DeleteOneAsync(u => u.Id == id);
    }
}
