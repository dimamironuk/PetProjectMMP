using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.15f;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float hoverOffsetY = 35f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, duration).SetEase(Ease.OutQuad);
        transform.DOLocalMoveY(hoverOffsetY, duration).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originalScale, duration).SetEase(Ease.OutQuad);
        transform.DOLocalMoveY(0f, duration).SetEase(Ease.OutQuad);
    }
}