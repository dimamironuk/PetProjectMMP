public interface ICardPlayHandler
{
    bool DiscardAfterPlay { get; }

    bool CanHandle(CardData card);
    bool CanPlay(CardData card, PlayTarget target);
    void Play(CardData card, PlayTarget target);
}