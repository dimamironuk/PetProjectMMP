using System.Threading.Tasks;

public interface IAuthService
{
    bool IsLoggedIn { get; }
    string PlayFabId { get; }

    Task<AuthResult> RegisterAsync(string username, string email, string password);
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> SendRecoveryEmailAsync(string email);
}