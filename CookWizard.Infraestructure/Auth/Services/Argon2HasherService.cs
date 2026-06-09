using CookWizard.Domain.Auth.Services;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace CookWizard.Infrastructure.Auth.Services;

public class Argon2HasherService : IPasswordHasherService
{
    public byte[] GenerateSalt(int length)
    {
        var buffer = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(buffer);
        }
        return buffer;
    }

    public byte[] HashPassword(string password, byte[] salt)
    {

        using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
        {
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; // Number of threads
            argon2.MemorySize = 65536; // 64 MB of memory
            argon2.Iterations = 4; // Number of iterations

            return argon2.GetBytes(32); // Get 32-byte hash
        }
    }

    public bool VerifyPassword(string enteredPassword, byte[] storedSalt, byte[] storedHash)
    {
        var enteredHash = HashPassword(enteredPassword, storedSalt);

        return CryptographicOperations.FixedTimeEquals(enteredHash, storedHash);
    }
}
