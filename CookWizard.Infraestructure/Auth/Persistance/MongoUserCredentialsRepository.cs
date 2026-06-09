using CookWizard.Domain.Auth.Models;
using CookWizard.Domain.Auth.Repository;
using CookWizard.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CookWizard.Infrastructure.Auth.Persistance;

public class MongoUserCredentialsRepository : IUserCredentialsRepository
{
    private readonly IMongoCollection<UserCredentials> _userCredentials;

    public MongoUserCredentialsRepository(
        IMongoClient client,
        IOptions<MongoSettings> options
    )
    {
        var settings = options.Value;
        var database = client.GetDatabase("CookWizard");

        _userCredentials = database.GetCollection<UserCredentials>("UserCredentials");
    }

    public async Task AddAsync(UserCredentials userCredentials)
    {
        await _userCredentials.InsertOneAsync(userCredentials);
    }

    public async Task<UserCredentials> GetCredentialsByEmail(string email)
    {
        return await _userCredentials.Find(x => x.Email == email)
            .FirstOrDefaultAsync();
    }

}
