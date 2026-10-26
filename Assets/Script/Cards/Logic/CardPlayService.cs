using System;
using System.Collections.Generic;

public enum PlayRejectReason
{
    NotEnoughTokens,
    InvalidTarget
}

public class CardPlayService
{
    private readonly ITokenPool tokens;
    private readonly IDeck deck;
    private readonly IHand hand;
    private readonly List<ICardPlayHandler> handlers;

    public event Action<CardData> CardPlayed;
    public event Action<CardData, PlayRejectReason> PlayRejected;

    public CardPlayService(ITokenPool tokens, IDeck deck, IHand hand, List<ICardPlayHandler> handlers)
    {
        this.tokens = tokens;
        this.deck = deck;
        this.hand = hand;
        this.handlers = handlers;
    }

    public bool TryPlay(CardUI cardUI, PlayTarget target)
    {
        if (cardUI == null) return false;

        CardData card = cardUI.CardData;
        if (card == null) return false;

        ICardPlayHandler handler = handlers.Find(h => h.CanHandle(card));

        if (handler == null || !handler.CanPlay(card, target))
            return Reject(card, PlayRejectReason.InvalidTarget);

        if (!tokens.TryPay(card.Cost))
            return Reject(card, PlayRejectReason.NotEnoughTokens);

        handler.Play(card, target);
        hand.Remove(cardUI);

        if (handler.DiscardAfterPlay) deck.Discard(card);

        CardPlayed?.Invoke(card);
        return true;
    }

    private bool Reject(CardData card, PlayRejectReason reason)
    {
        PlayRejected?.Invoke(card, reason);
        return false;
    }
}