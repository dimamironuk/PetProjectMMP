public readonly struct PlayTarget
{
    public CharacterSlot Slot { get; }
    public CharacterUnit Unit { get; }

    private PlayTarget(CharacterSlot slot, CharacterUnit unit)
    {
        Slot = slot;
        Unit = unit;
    }

    public static PlayTarget None => default;
    public static PlayTarget OnSlot(CharacterSlot slot) => new PlayTarget(slot, null);
    public static PlayTarget OnUnit(CharacterUnit unit) => new PlayTarget(null, unit);
}