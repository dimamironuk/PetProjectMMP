using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private List<CardData> initialDeck = new List<CardData>();

    private List<CardData> drawPile = new List<CardData>();
    private List<CardData> discardPile = new List<CardData>();

    private void Awake() => InitializeDeck();

    public void InitializeDeck()
    {
        drawPile = new List<CardData>(initialDeck);
        Shuffle();
    }

    public void Shuffle()
    {
        for (int i = 0; i < drawPile.Count; i++)
        {
            var temp = drawPile[i];
            int randomIndex = Random.Range(i, drawPile.Count);
            drawPile[i] = drawPile[randomIndex];
            drawPile[randomIndex] = temp;
        }
    }

    public CardData DrawCard()
    {
        if (drawPile.Count == 0)
        {
            if (discardPile.Count == 0) return null;

            drawPile.AddRange(discardPile);
            discardPile.Clear();
            Shuffle();
        }

        CardData drawnCard = drawPile[0];
        drawPile.RemoveAt(0);
        return drawnCard;
    }

    public void Discard(CardData card) => discardPile.Add(card);
}