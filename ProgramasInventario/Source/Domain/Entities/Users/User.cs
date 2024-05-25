namespace Domain.Entities.Users;

public class User : BaseEntity
{
    public string Email { get; }
    public string Password { get; private set; }
    public byte[] Key { get; private set; }

    private User()
    {
    }

    public User(string email, string password, byte[] key, string requestedBy)
    {
        SetCreate(requestedBy);
        Email = email;
        Password = password;
        Key = key;
    }

    public void Inactivate(string requestedBy) => SetInactive(requestedBy);

    public void Update(string password, byte[] key, string requestedBy)
    {
        SetUpdate(requestedBy);
        Password = password;
        Key = key;
    }
}