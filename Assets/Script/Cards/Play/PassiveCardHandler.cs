public class PassiveCardHandler : CardPlayHandler<PassiveCardData>
{
    private readonly IBoard board;

    public PassiveCardHandler(IBoard board)
    {
        this.board = board;
    }

    protected override bool CanPlayCard(PassiveCardData card, PlayTarget target)
        => target.Unit != null &&
           target.Unit.CanAttachPassive &&
           board.HasUnit(target.Unit);

    protected override void PlayCard(PassiveCardData card, PlayTarget target)
        => target.Unit.TryAttachPassive(card);
}
