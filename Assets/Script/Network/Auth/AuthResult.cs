public readonly struct AuthResult
{
    public bool Success { get; }
    public string Error { get; }

    private AuthResult(bool success, string error)
    {
        Success = success;
        Error = error;
    }

    public static AuthResult Ok() => new AuthResult(true, null);
    public static AuthResult Fail(string error) => new AuthResult(false, error);
}