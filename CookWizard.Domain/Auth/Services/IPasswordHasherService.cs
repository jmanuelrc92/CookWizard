namespace CookWizard.Domain.Auth.Services;

public interface IPasswordHasherService
{
   byte[] HashPassword(string password, byte[] salt);
   byte[] GenerateSalt(int length);
   bool VerifyPassword(string enteredPassword, byte[] storedSalt, byte[] storedHash);
}