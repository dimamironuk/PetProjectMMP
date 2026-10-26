public class CharacterCardHandler : CardPlayHandler<CharacterCardData>
{
    private readonly IBoard board;

    public CharacterCardHandler(IBoard board)
    {
        this.board = board;
    }

    public override bool DiscardAfterPlay => false;

    protected override bool CanPlayCard(CharacterCardData card, PlayTarget target)
        => target.Slot != null && target.Slot.IsEmpty && board.HasSlot(target.Slot);

    protected override void PlayCard(CharacterCardData card, PlayTarget target)
        => target.Slot.Place(card);
}
