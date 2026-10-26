using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour, IDeck
{
    [SerializeField] private List<CardData> initialDeck = new();

    private readonly List<CardData> drawPile = new();
    private readonly List<CardData> discardPile = new();
    private System.Random rng;

    private void Awake() => Initialize(System.Environment.TickCount);

    public void Initialize(int seed)
    {
        rng = new System.Random(seed);
        drawPile.Clear();
        discardPile.Clear();
        drawPile.AddRange(initialDeck);
        Shuffle(drawPile);
    }

    public CardData Draw()
    {
        if (drawPile.Count == 0)
        {
            if (discardPile.Count == 0) return null;
            drawPile.AddRange(discardPile);
            discardPile.Clear();
            Shuffle(drawPile);
        }

        int last = drawPile.Count - 1;  
        var card = drawPile[last];
        drawPile.RemoveAt(last);
        return card;
    }

    public void Discard(CardData card)
    {
        if (card != null) discardPile.Add(card);
    }

    private void Shuffle(List<CardData> list)   
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}