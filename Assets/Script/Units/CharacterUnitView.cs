using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUnitView : MonoBehaviour
{
    [SerializeField] private CharacterUnit unit;
    [SerializeField] private RectTransform visualContainer;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Image portrait;

    private float lastHp = -1f;

    private void OnEnable()
    {
        if (unit == null) return;
        unit.Initialized += OnInitialized;
        unit.HpChanged += OnHpChanged;
    }

    private void OnDisable()
    {
        if (unit != null)
        {
            unit.Initialized -= OnInitialized;
            unit.HpChanged -= OnHpChanged;
        }
        DOTween.Kill(this, true);
    }

    private void OnInitialized(CharacterCardData data)
    {
        if (data == null) return;
        if (nameText != null) nameText.text = data.CardName;
        if (portrait != null) portrait.sprite = data.Icon;
    }

    private void OnHpChanged(float current, float max)
    {
        if (hpText != null) hpText.text = Mathf.CeilToInt(current).ToString();

        if (lastHp > current) PlayHitAnimation();
        lastHp = current;
    }

    private void PlayHitAnimation()
    {
        if (visualContainer == null) return;
        DOTween.Kill(this, true);
        visualContainer.DOShakeAnchorPos(0.3f, 12f, 20).SetId(this);
    }
}