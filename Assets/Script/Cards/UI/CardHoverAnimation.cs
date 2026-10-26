using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform visualContainer; 
    [SerializeField] private float hoverScale = 1.15f;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float hoverOffsetY = 35f;

    private Vector3 originalScale = Vector3.one;

    private void Awake()
    {
        if (visualContainer != null)
            originalScale = visualContainer.localScale;
    }

    private void OnDisable() => ResetVisual(true);

    public void OnPointerEnter(PointerEventData e)
    {
        if (!isActiveAndEnabled) return;
        Animate(originalScale * hoverScale, hoverOffsetY);
    }

    public void OnPointerExit(PointerEventData e) => Animate(originalScale, 0f);

    public void ResetVisual(bool immediate)
    {
        if (visualContainer == null) return;

        DOTween.Kill(this);
        if (immediate)
        {
            visualContainer.localScale = originalScale;
            visualContainer.anchoredPosition = new Vector2(visualContainer.anchoredPosition.x, 0f);
            return;
        }

        Animate(originalScale, 0f);
    }

    private void Animate(Vector3 scale, float y)
    {
        if (visualContainer == null) return;

        DOTween.Kill(this);
        visualContainer.DOScale(scale, duration).SetEase(Ease.OutQuad).SetId(this);
        visualContainer.DOAnchorPosY(y, duration).SetEase(Ease.OutQuad).SetId(this);
    }

    private void OnDestroy() => DOTween.Kill(this);
}