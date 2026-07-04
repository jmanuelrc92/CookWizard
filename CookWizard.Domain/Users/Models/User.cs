namespace CookWizard.Domain.Users.Models;

public class User
{
    public string Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public DateOnly Birthday { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public User(
        string username,
        string firstName,
        string lastName,
        string email,
        DateOnly birthday
    )
    {
        Id = Guid.NewGuid().ToString();
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Birthday = birthday;
        CreatedAt = DateTime.UtcNow;
    }
}