using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignInPanel : AuthPanelBase
{
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button toSignUpButton;
    [SerializeField] private Button forgotPasswordButton;

    public event Action<string, string> SignInClicked;
    public event Action<string> ForgotPasswordClicked;
    public event Action ToSignUpClicked;

    private void OnEnable()
    {
        signInButton.onClick.AddListener(OnSignInClicked);
        forgotPasswordButton.onClick.AddListener(OnForgotPasswordClicked);
        toSignUpButton.onClick.AddListener(OnToSignUpClicked);
    }

    private void OnDisable()
    {
        signInButton.onClick.RemoveListener(OnSignInClicked);
        forgotPasswordButton.onClick.RemoveListener(OnForgotPasswordClicked);
        toSignUpButton.onClick.RemoveListener(OnToSignUpClicked);
    }

    private void OnSignInClicked() =>
        SignInClicked?.Invoke(emailInput.text.Trim(), passwordInput.text);

    private void OnForgotPasswordClicked() =>
        ForgotPasswordClicked?.Invoke(emailInput.text.Trim());

    private void OnToSignUpClicked() =>
        ToSignUpClicked?.Invoke();
}