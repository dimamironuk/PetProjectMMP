public class ActionCardHandler : CardPlayHandler<ActionCardData>
{
    private readonly IBoard board;

    public ActionCardHandler(IBoard board)
    {
        this.board = board;
    }

    protected override bool CanPlayCard(ActionCardData card, PlayTarget target)
    {
        switch (card.TargetType)
        {
            case CardTargetType.SingleAlly:
                return IsOwnAliveUnit(target.Unit);
            case CardTargetType.AllAllies:
                foreach (var _ in board.Characters)
                    return true;
                return false;
            default:
                return false;
        }
    }

    protected override void PlayCard(ActionCardData card, PlayTarget target)
    {
        if (card.TargetType == CardTargetType.AllAllies)
        {
            foreach (var unit in board.Characters)
                ApplyEffects(card, unit);
            return;
        }

        if (IsOwnAliveUnit(target.Unit))
            ApplyEffects(card, target.Unit);
    }

    private bool IsOwnAliveUnit(CharacterUnit unit)
        => unit != null && unit.IsAlive && board.HasUnit(unit);

    private static void ApplyEffects(ActionCardData card, CharacterUnit unit)
    {
        if (unit == null || card.Effects == null) return;

        var context = new ReactionContext(unit.gameObject, unit.transform.position, unit.transform.forward);

        foreach (var effect in card.Effects)
            if (effect != null) effect.Apply(context);
    }
}
