using UnityEngine;

public class AuthFlow : MonoBehaviour
{
    [SerializeField] private AuthPopupController popup;
    [SerializeField] private GameObject mainMenuPanel;

    private void OnEnable() => popup.Authenticated += OnAuthenticated;
    private void OnDisable() => popup.Authenticated -= OnAuthenticated;

    private void OnAuthenticated()
    {
        popup.gameObject.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}