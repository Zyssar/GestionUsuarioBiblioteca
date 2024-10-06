using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionUsuarioBiblioteca
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _users = database.GetCollection<User>(mongoDbSettings.Value.CollectionName);
        }

        public async Task<List<User>> GetAsync() =>
            await _users.Find(user => true).ToListAsync();

        public async Task<User> GetByIdAsync(int id) =>
            await _users.Find(user => user.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User user)
        {
            // Obtener el siguiente Id
            var lastUser = await _users.Find(user => true).SortByDescending(u => u.Id).FirstOrDefaultAsync();
            user.Id = lastUser != null ? lastUser.Id + 1 : 0;  // Asignar Id secuencial

            await _users.InsertOneAsync(user);
        }

        public async Task UpdateAsync(int id, User user) =>
            await _users.ReplaceOneAsync(existingUser => existingUser.Id == id, user);

        public async Task DeleteAsync(int id) =>
            await _users.DeleteOneAsync(user => user.Id == id);
    }
}
