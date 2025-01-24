using System.Text.RegularExpressions;
namespace AuthenticationService;

public partial class CredentialsValidator : ICredentialsValidator
{
    public bool ValidateUsername(string username)
    {
        var match = UserNameRegex().Match(username);
        return match.Success;
    }

    public bool ValidatePassword(string password)
    {
        var match = PasswordRegex().Match(password);
        return match.Success;
    }

    private static Regex UserNameRegex() => new Regex("^[a-zA-Z][a-zA-Z0-9_]{7,31}$", RegexOptions.Compiled);
    private static Regex PasswordRegex() => new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{}|\\,.<>?]).{8,16}$", RegexOptions.Compiled);
}