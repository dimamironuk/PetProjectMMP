using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CardUI), typeof(CanvasGroup), typeof(LayoutElement))]
public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CardHoverAnimation hover;

    [Inject] private CardPlayService playService;

    private readonly List<RaycastResult> hits = new List<RaycastResult>();

    private CardUI card;
    private CanvasGroup group;
    private LayoutElement layoutElement;
    private RectTransform rect;
    private RectTransform canvasRect;
    private Transform homeParent;
    private int homeIndex;
    private Vector2 size;

    private void Awake()
    {
        card = GetComponent<CardUI>();
        group = GetComponent<CanvasGroup>();
        layoutElement = GetComponent<LayoutElement>();
        rect = (RectTransform)transform;
        if (hover == null) hover = GetComponent<CardHoverAnimation>();

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null) canvasRect = (RectTransform)canvas.rootCanvas.transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvasRect == null)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas == null) return;
            canvasRect = (RectTransform)canvas.rootCanvas.transform;
        }

        homeParent = rect.parent;
        homeIndex = rect.GetSiblingIndex();
        size = rect.rect.size;

        layoutElement.ignoreLayout = true;
        rect.SetParent(canvasRect, true);
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        group.blocksRaycasts = false;

        if (hover != null)
        {
            hover.ResetVisual(true);
            hover.enabled = false;
        }

        MoveTo(eventData);
    }

    public void OnDrag(PointerEventData eventData) => MoveTo(eventData);

    public void OnEndDrag(PointerEventData eventData)
    {
        bool overHand = homeParent is RectTransform homeRect &&
            RectTransformUtility.RectangleContainsScreenPoint(homeRect, eventData.position, eventData.pressEventCamera);

        PlayTarget target = FindTarget(eventData);
        group.blocksRaycasts = true;

        if (!overHand && playService != null && playService.TryPlay(card, target))
            return;

        ReturnToHand();
    }

    private void MoveTo(PointerEventData eventData)
    {
        if (canvasRect == null) return;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvasRect, eventData.position, eventData.pressEventCamera, out Vector3 world))
            rect.position = world;
    }

    private PlayTarget FindTarget(PointerEventData eventData)
    {
        hits.Clear();
        if (EventSystem.current == null) return PlayTarget.None;

        EventSystem.current.RaycastAll(eventData, hits);

        foreach (var hit in hits)
        {
            if (hit.gameObject == null || hit.gameObject.transform.IsChildOf(transform)) continue;

            var unit = hit.gameObject.GetComponentInParent<CharacterUnit>();
            if (unit != null && unit.IsAlive) return PlayTarget.OnUnit(unit);

            var slot = hit.gameObject.GetComponentInParent<CharacterSlot>();
            if (slot != null) return PlayTarget.OnSlot(slot);
        }

        return PlayTarget.None;
    }

    private void ReturnToHand()
    {
        if (homeParent != null)
        {
            rect.SetParent(homeParent, false);
            rect.SetSiblingIndex(homeIndex);
        }

        layoutElement.ignoreLayout = false;
        if (hover == null) return;

        hover.enabled = true;
        hover.ResetVisual(true);
    }
}