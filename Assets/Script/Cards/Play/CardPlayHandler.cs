public abstract class CardPlayHandler<TCard> : ICardPlayHandler where TCard : CardData
{
    public virtual bool DiscardAfterPlay => true;

    public bool CanHandle(CardData card) => card is TCard;
    public bool CanPlay(CardData card, PlayTarget target) => CanPlayCard((TCard)card, target);
    public void Play(CardData card, PlayTarget target) => PlayCard((TCard)card, target);

    protected abstract bool CanPlayCard(TCard card, PlayTarget target);
    protected abstract void PlayCard(TCard card, PlayTarget target);
}