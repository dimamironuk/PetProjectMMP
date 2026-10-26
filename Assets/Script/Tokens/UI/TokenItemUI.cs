using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TokenItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;

    private ElementData shown;

    public void Show(ElementData element)
    {
        icon.sprite = element.elementIcon;

        if (shown == element) return;
        shown = element;

        transform.DOKill(true);
        transform.DOPunchScale(Vector3.one * 0.2f, 0.25f, 6);
    }

    private void OnDisable()
    {
        shown = null;
        transform.DOKill(true);
    }
}