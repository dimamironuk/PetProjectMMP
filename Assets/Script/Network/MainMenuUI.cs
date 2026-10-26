using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private NetworkLauncher launcher;
    [SerializeField] private Button playButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TMP_Text statusText;

    [Header("Status")]
    [SerializeField] private string defaultSearchText = "Searching for player...";
    [SerializeField] private CanvasGroup statusBackground; 
    [SerializeField] private float statusBgFadeDuration = 0.25f;

    [SerializeField] private CanvasGroup[] hideWhileSearching; 
    [SerializeField] private float uiFadeDuration = 0.25f;

    [Header("Search icon")]
    [SerializeField] private RectTransform searchIcon;
    [SerializeField] private float figureWidth = 70f;
    [SerializeField] private float figureHeight = 35f;
    [SerializeField] private float loopDuration = 2.5f;
    [SerializeField] private float tilt = 8f;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField, Range(0f, 1f)] private float fadeAlpha = 0.6f;
    [SerializeField] private float fadeDuration = 0.35f;

    private Vector2 iconBasePosition;

    private void Awake()
    {
        playButton.onClick.AddListener(launcher.FindMatch);
        cancelButton.onClick.AddListener(launcher.CancelSearch);
        launcher.StatusChanged += OnStatusChanged;
        launcher.SearchStateChanged += OnSearchStateChanged;

        if (searchIcon != null) iconBasePosition = searchIcon.anchoredPosition;

        if (fadeOverlay != null) fadeOverlay.alpha = 0f;

        if (statusBackground != null)
        {
            statusBackground.alpha = 0f;
            statusBackground.gameObject.SetActive(false);
        }

        foreach (var group in hideWhileSearching)
        {
            if (group == null) continue;
            group.alpha = 1f;
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        statusText.text = string.Empty;
        OnSearchStateChanged(false);
    }

    private void OnDestroy()
    {
        launcher.StatusChanged -= OnStatusChanged;
        launcher.SearchStateChanged -= OnSearchStateChanged;
        DOTween.Kill(this);
        if (fadeOverlay != null) fadeOverlay.DOKill();
        if (statusBackground != null) statusBackground.DOKill();
        foreach (var group in hideWhileSearching)
            if (group != null) group.DOKill();
    }

    private void OnStatusChanged(string text) => statusText.text = text;

    private void OnSearchStateChanged(bool searching)
    {
        cancelButton.gameObject.SetActive(searching);

        statusText.text = searching ? defaultSearchText : string.Empty;

        SetSearchIcon(searching);
        SetDim(searching);
        SetStatusBackground(searching);
        SetMainUiHidden(searching);
    }

    private void SetMainUiHidden(bool hidden)
    {
        foreach (var group in hideWhileSearching)
        {
            if (group == null) continue;

            group.DOKill();
            group.interactable = !hidden;
            group.blocksRaycasts = !hidden;
            group.DOFade(hidden ? 0f : 1f, uiFadeDuration);
        }
    }

    private void SetStatusBackground(bool visible)
    {
        if (statusBackground == null) return;

        statusBackground.DOKill();

        if (visible)
        {
            statusBackground.gameObject.SetActive(true);
            statusBackground.DOFade(1f, statusBgFadeDuration);
        }
        else
        {
            statusBackground.DOFade(0f, statusBgFadeDuration)
                            .OnComplete(() => statusBackground.gameObject.SetActive(false));
        }
    }

    private void SetDim(bool visible)
    {
        if (fadeOverlay == null) return;
        fadeOverlay.DOKill();
        fadeOverlay.DOFade(visible ? fadeAlpha : 0f, fadeDuration);
    }

    private void SetSearchIcon(bool visible)
    {
        if (searchIcon == null) return;

        DOTween.Kill(this);
        searchIcon.localRotation = Quaternion.identity;
        searchIcon.anchoredPosition = iconBasePosition;
        searchIcon.gameObject.SetActive(visible);
        if (!visible) return;

        DOVirtual.Float(0f, Mathf.PI * 2f, loopDuration, MoveAlongFigureEight)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart)
                 .SetId(this);
    }

    private void MoveAlongFigureEight(float t)
    {
        float x = Mathf.Sin(t) * figureWidth;
        float y = Mathf.Sin(2f * t) * figureHeight;
        searchIcon.anchoredPosition = iconBasePosition + new Vector2(x, y);
        searchIcon.localRotation = Quaternion.Euler(0f, 0f, -Mathf.Sin(t) * tilt);
    }
}