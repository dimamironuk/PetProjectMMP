using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanel : AuthPanelBase
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button toSignInButton;

    public event Action<string, string, string> SignUpClicked;
    public event Action ToSignInClicked;

    private void OnEnable()
    {
        signUpButton.onClick.AddListener(OnSignUpClicked);
        toSignInButton.onClick.AddListener(OnToSignInClicked);
    }

    private void OnDisable()
    {
        signUpButton.onClick.RemoveListener(OnSignUpClicked);
        toSignInButton.onClick.RemoveListener(OnToSignInClicked);
    }

    private void OnSignUpClicked() =>
        SignUpClicked?.Invoke(usernameInput.text.Trim(), emailInput.text.Trim(), passwordInput.text);

    private void OnToSignInClicked() =>
        ToSignInClicked?.Invoke();
}