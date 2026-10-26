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
    [SerializeField] private Image iconImage;

    [Header("Cost")]
    [SerializeField] private GameObject elementCostRoot;
    [SerializeField] private Image elementCostIcon;
    [SerializeField] private TMP_Text elementCostText;
    [SerializeField] private GameObject genericCostRoot;
    [SerializeField] private TMP_Text genericCostText;

    [Header("Character Only")]
    [SerializeField] private GameObject hpRoot;
    [SerializeField] private TMP_Text hpText;

    public CardData CardData { get; private set; }

    public void Setup(CardData data)
    {
        CardData = data;

        if (nameText != null) nameText.text = data.CardName;
        if (iconImage != null) iconImage.sprite = data.Icon;

        ShowCost(data.Cost);
        ShowHp(data as CharacterCardData);
    }

    public void AnimateSpawn(Vector3 startWorldPosition)
    {
        if (visualContainer == null) return;

        visualContainer.position = startWorldPosition;
        visualContainer.localScale = Vector3.zero;

        if (canvasGroup != null) canvasGroup.alpha = 0f;

        Sequence sequence = DOTween.Sequence().SetLink(gameObject);
        if (canvasGroup != null) sequence.Append(canvasGroup.DOFade(1f, 0.2f));

        sequence.Join(visualContainer.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack));
        sequence.Join(visualContainer.DOLocalMove(Vector3.zero, 0.35f).SetEase(Ease.OutCubic));
    }

    private void ShowCost(CardCost cost)
    {
        bool hasElement = cost.elementAmount > 0;
        bool hasGeneric = cost.genericAmount > 0;

        SetActive(elementCostRoot, hasElement);
        SetActive(genericCostRoot, hasGeneric);

        if (hasElement)
        {
            if (elementCostText != null) elementCostText.text = cost.elementAmount.ToString();
            if (elementCostIcon != null) elementCostIcon.sprite = cost.element != null ? cost.element.elementIcon : null;
        }

        if (hasGeneric && genericCostText != null)
            genericCostText.text = cost.genericAmount.ToString();
    }

    private void ShowHp(CharacterCardData character)
    {
        SetActive(hpRoot, character != null);
        if (character != null && hpText != null) hpText.text = character.MaxHp.ToString();
    }

    private static void SetActive(GameObject target, bool value)
    {
        if (target != null) target.SetActive(value);
    }
}