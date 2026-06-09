namespace CookWizard.Domain.Auth.Models;

public class UserCredentials
{
    public string UserId { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Salt {  get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private UserCredentials() { }

    public UserCredentials(
        string userId,
        string email,
        string passwordHash,
        string salt
    )
    {
        UserId = userId;
        Email = email;
        PasswordHash = passwordHash;
        Salt = salt;
        ModifiedAt = DateTime.UtcNow;
    }
}
