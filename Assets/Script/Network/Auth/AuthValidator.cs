using System.Text.RegularExpressions;

public static class AuthValidator
{
    private const int MinUsernameLength = 3;
    private const int MaxUsernameLength = 20;
    private const int MinPasswordLength = 6;

    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public static bool TryValidateEmail(string email, out string error)
    {
        error = EmailRegex.IsMatch(email ?? string.Empty) ? null : "Введи коректну пошту";
        return error == null;
    }

    public static bool TryValidateSignIn(string email, string password, out string error)
    {
        if (!TryValidateEmail(email, out error)) return false;

        error = string.IsNullOrEmpty(password) ? "Введи пароль" : null;
        return error == null;
    }

    public static bool TryValidateSignUp(string username, string email, string password, out string error)
    {
        if (string.IsNullOrWhiteSpace(username) ||
            username.Length < MinUsernameLength ||
            username.Length > MaxUsernameLength)
        {
            error = $"Нікнейм: від {MinUsernameLength} до {MaxUsernameLength} символів";
            return false;
        }

        if (!TryValidateEmail(email, out error)) return false;

        error = string.IsNullOrEmpty(password) || password.Length < MinPasswordLength
            ? $"Пароль: мінімум {MinPasswordLength} символів"
            : null;
        return error == null;
    }
}