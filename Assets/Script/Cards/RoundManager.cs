using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private int cardsToDrawPerRound = 4;

    [Inject] private DeckManager deckManager;
    [Inject] private HandManager handManager;

    private void Start()
    {
        StartNewRound();
    }

    public void StartNewRound()
    {
        StartCoroutine(DrawRoundCardsRoutine());
    }

    private IEnumerator DrawRoundCardsRoutine()
    {
        List<CardData> drawnCards = new List<CardData>();

        for (int i = 0; i < cardsToDrawPerRound; i++)
        {
            CardData card = deckManager.DrawCard();
            if (card != null)
            {
                drawnCards.Add(card);
            }
        }

        yield return handManager.DrawCardsRoutine(drawnCards);
    }
}