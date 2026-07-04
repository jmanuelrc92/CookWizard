using CookWizard.Domain.Users.Models;
using CookWizard.Domain.Users.Repository;
using CookWizard.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CookWizard.Infrastructure.Users.Persistance;

public class MongoUserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public MongoUserRepository(
        IMongoClient client,
        IOptions<MongoSettings> options
    )
    {
        var settings = options.Value;
        var database = client.GetDatabase("CookWizard");
        _users = database.GetCollection<User>("Users");
    }

    public async Task AddAsync(User user)
    {
        await _users.InsertOneAsync(user);
    }

    public async Task<bool> EmailExistAsync(string email)
    {
        return await _users
            .Find(x => x.Email == email)
            .AnyAsync();
    }

    public async Task<User> GetByIdAsync(string guid)
    {
        return await _users.Find(u => u.Id == guid)
            .FirstOrDefaultAsync();
    }
}
