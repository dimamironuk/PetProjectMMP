using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabAuthService : IAuthService
{
    public bool IsLoggedIn => PlayFabClientAPI.IsClientLoggedIn();
    public string PlayFabId { get; private set; }

    public Task<AuthResult> RegisterAsync(string username, string email, string password)
    {
        var tcs = new TaskCompletionSource<AuthResult>();

        var request = new RegisterPlayFabUserRequest
        {
            Username = username,
            DisplayName = username,
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = true
        };

        PlayFabClientAPI.RegisterPlayFabUser(
            request,
            result =>
            {
                PlayFabId = result.PlayFabId;
                tcs.SetResult(AuthResult.Ok());
            },
            error => tcs.SetResult(AuthResult.Fail(AuthErrorMapper.Map(error))));

        return tcs.Task;
    }

    public Task<AuthResult> LoginAsync(string email, string password)
    {
        var tcs = new TaskCompletionSource<AuthResult>();

        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(
            request,
            result =>
            {
                PlayFabId = result.PlayFabId;
                tcs.SetResult(AuthResult.Ok());
            },
            error => tcs.SetResult(AuthResult.Fail(AuthErrorMapper.Map(error))));

        return tcs.Task;
    }

    public Task<AuthResult> SendRecoveryEmailAsync(string email)
    {
        var tcs = new TaskCompletionSource<AuthResult>();

        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = PlayFabSettings.TitleId
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(
            request,
            _ => tcs.SetResult(AuthResult.Ok()),
            error => tcs.SetResult(AuthResult.Fail(AuthErrorMapper.Map(error))));

        return tcs.Task;
    }
}