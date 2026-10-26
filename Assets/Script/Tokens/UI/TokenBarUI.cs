using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class TokenBarUI : MonoBehaviour
{
    [SerializeField] private TokenItemUI itemPrefab;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private TMP_Text countText;

    [Inject] private ITokenPool pool;

    private readonly List<TokenItemUI> items = new();

    private void OnEnable()
    {
        pool.Changed += Refresh;
        Refresh();
    }

    private void OnDisable() => pool.Changed -= Refresh;

    private void Refresh()
    {
        var tokens = pool.Tokens;
        countText.text = tokens.Count.ToString();

        while (items.Count < tokens.Count)
            items.Add(Instantiate(itemPrefab, itemsContainer));

        for (int i = 0; i < items.Count; i++)
        {
            bool active = i < tokens.Count;
            items[i].gameObject.SetActive(active);
            if (active) items[i].Show(tokens[i]);
        }
    }
}