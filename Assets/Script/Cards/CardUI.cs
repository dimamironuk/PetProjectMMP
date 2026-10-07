using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [Header("Animation Container")]
    [SerializeField] private RectTransform visualContainer;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image iconImage;

    public CardData CardData { get; private set; }

    public void Setup(CardData data)
    {
        CardData = data;
        if (nameText != null) nameText.text = data.cardName;
        if (costText != null) costText.text = data.energyCost.ToString();
        if (iconImage != null) iconImage.sprite = data.cardIcon;
    }

    public void AnimateSpawn(Vector3 startWorldPosition)
    {
        if (visualContainer == null) return;

        visualContainer.position = startWorldPosition;
        visualContainer.localScale = Vector3.zero;

        if (canvasGroup != null) canvasGroup.alpha = 0f;

        Sequence sequence = DOTween.Sequence();
        if (canvasGroup != null) sequence.Append(canvasGroup.DOFade(1f, 0.2f));

        sequence.Join(visualContainer.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack));
        sequence.Join(visualContainer.DOLocalMove(Vector3.zero, 0.35f).SetEase(Ease.OutCubic));
    }
}