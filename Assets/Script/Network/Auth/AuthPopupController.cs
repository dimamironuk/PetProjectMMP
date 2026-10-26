using System;
using UnityEngine;
using Zenject;

public class AuthPopupController : MonoBehaviour
{
    [SerializeField] private SignInPanel signInPanel;
    [SerializeField] private SignUpPanel signUpPanel;

    private IAuthService authService;

    public event Action Authenticated;

    [Inject]
    private void Construct(IAuthService authService)
    {
        this.authService = authService;
    }

    private void OnEnable()
    {
        signInPanel.SignInClicked += HandleSignIn;
        signInPanel.ForgotPasswordClicked += HandleForgotPassword;
        signInPanel.ToSignUpClicked += ShowSignUp;

        signUpPanel.SignUpClicked += HandleSignUp;
        signUpPanel.ToSignInClicked += ShowSignIn;
    }

    private void OnDisable()
    {
        signInPanel.SignInClicked -= HandleSignIn;
        signInPanel.ForgotPasswordClicked -= HandleForgotPassword;
        signInPanel.ToSignUpClicked -= ShowSignUp;

        signUpPanel.SignUpClicked -= HandleSignUp;
        signUpPanel.ToSignInClicked -= ShowSignIn;
    }

    private void Start()
    {
        signUpPanel.gameObject.SetActive(false);
        signInPanel.Show();
    }

    private void ShowSignIn()
    {
        signUpPanel.Hide();
        signInPanel.Show();
    }

    private void ShowSignUp()
    {
        signInPanel.Hide();
        signUpPanel.Show();
    }

    private async void HandleSignIn(string email, string password)
    {
        if (!AuthValidator.TryValidateSignIn(email, password, out string error))
        {
            signInPanel.SetMessage(error);
            return;
        }

        signInPanel.SetBusy(true);
        AuthResult result = await authService.LoginAsync(email, password);
        if (!isActiveAndEnabled) return;

        signInPanel.SetBusy(false);

        if (!result.Success)
        {
            signInPanel.SetMessage(result.Error);
            return;
        }

        Authenticated?.Invoke();
    }

    private async void HandleSignUp(string username, string email, string password)
    {
        if (!AuthValidator.TryValidateSignUp(username, email, password, out string error))
        {
            signUpPanel.SetMessage(error);
            return;
        }

        signUpPanel.SetBusy(true);
        AuthResult result = await authService.RegisterAsync(username, email, password);
        if (!isActiveAndEnabled) return;

        signUpPanel.SetBusy(false);

        if (!result.Success)
        {
            signUpPanel.SetMessage(result.Error);
            return;
        }

        Authenticated?.Invoke();
    }

    private async void HandleForgotPassword(string email)
    {
        if (!AuthValidator.TryValidateEmail(email, out string error))
        {
            signInPanel.SetMessage(error);
            return;
        }

        signInPanel.SetBusy(true);
        AuthResult result = await authService.SendRecoveryEmailAsync(email);
        if (!isActiveAndEnabled) return;

        signInPanel.SetBusy(false);
        signInPanel.SetMessage(result.Success ? "Лист для відновлення надіслано" : result.Error);
    }
}