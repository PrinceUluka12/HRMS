namespace HRMS.Application.Helpers;

public class PasswordHelper
{
    // Hash a password with a randomly generated salt
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    }

    // Verify a password against a hash
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}