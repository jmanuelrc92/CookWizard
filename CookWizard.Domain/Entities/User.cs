namespace CookWizard.Domain.Entities;

public class User
{
    public string Id { get; private set; }
    public string Email { get; private set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public User(string email, string username, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required");

        Email = email.Trim().ToLower();
        Username = username;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }
}