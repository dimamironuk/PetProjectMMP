using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HandManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handContainer;
    [SerializeField] private Transform deckSpawnPoint;
    [SerializeField] private float drawInterval = 0.15f;

    [Inject] private DiContainer container;

    public void AddCardToHand(CardData cardData)
    {
        if (cardData == null) return;

        GameObject newCardObj = container.InstantiatePrefab(cardPrefab, handContainer);

        if (newCardObj.TryGetComponent<CardUI>(out var cardUI))
        {
            cardUI.Setup(cardData);
            Vector3 startPos = deckSpawnPoint != null ? deckSpawnPoint.position : handContainer.position;
            cardUI.AnimateSpawn(startPos);
        }
    }

    public IEnumerator DrawCardsRoutine(List<CardData> cards)
    {
        foreach (var card in cards)
        {
            AddCardToHand(card);
            yield return new WaitForSeconds(drawInterval);
        }
    }
}