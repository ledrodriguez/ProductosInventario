namespace Application.Services.Users.Security;

public static class Validator
{
    public static void ValidateData(string email, string password)
    {
        Email.ValidateFormat(email);
        Password.ValidateFormat(password);
    }
}