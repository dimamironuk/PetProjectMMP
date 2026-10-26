using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HandManager : MonoBehaviour, IHand
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handContainer;
    [SerializeField] private Transform deckSpawnPoint;
    [SerializeField] private int maxHandSize = 7;
    [SerializeField] private float drawInterval = 0.15f;

    [Inject] private DiContainer container;

    private readonly List<CardUI> cards = new();
    private WaitForSeconds drawWait;

    public bool HasSpace => cards.Count < maxHandSize;

    public void Add(CardData data)
    {
        if (data == null || cardPrefab == null || handContainer == null || !HasSpace) return;

        var obj = container.InstantiatePrefab(cardPrefab, handContainer);
        if (!obj.TryGetComponent<CardUI>(out var ui)) { Destroy(obj); return; }

        ui.Setup(data);
        ui.AnimateSpawn(deckSpawnPoint != null ? deckSpawnPoint.position : handContainer.position);
        cards.Add(ui);
    }

    public void Remove(CardUI card)
    {
        if (card == null || !cards.Remove(card)) return;
        Destroy(card.gameObject);
    }

    public IEnumerator DrawRoutine(IDeck deck, int count)
    {
        if (deck == null) yield break;
        if (drawWait == null) drawWait = new WaitForSeconds(drawInterval);

        for (int i = 0; i < count && HasSpace; i++)
        {
            var card = deck.Draw();
            if (card == null) yield break;
            Add(card);
            if (drawInterval > 0f) yield return drawWait;
        }
    }
}