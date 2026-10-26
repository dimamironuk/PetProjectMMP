using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class AuthPanelBase : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float fadeDuration = 0.25f;

    private CanvasGroup canvasGroup;

    private CanvasGroup Group
    {
        get
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
            return canvasGroup;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SetMessage(string.Empty);

        Group.DOKill();
        Group.alpha = 0f;
        Group.interactable = true;
        Group.DOFade(1f, fadeDuration);
    }

    public void Hide()
    {
        if (!gameObject.activeSelf) return;

        Group.DOKill();
        Group.interactable = false;
        Group.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            if (this != null) gameObject.SetActive(false);
        });
    }

    public void SetBusy(bool isBusy)
    {
        Group.interactable = !isBusy;
    }

    public void SetMessage(string message)
    {
        if (messageText == null) return;

        messageText.text = message;
        messageText.transform.DOKill(true);

        if (!string.IsNullOrEmpty(message))
            messageText.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 8);
    }
}