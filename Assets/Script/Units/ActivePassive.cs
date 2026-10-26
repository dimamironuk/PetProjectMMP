public sealed class ActivePassive
{
    private readonly bool permanent;

    public PassiveCardData Data { get; }
    public int RemainingRounds { get; private set; }

    public ActivePassive(PassiveCardData data)
    {
        Data = data;
        permanent = data.DurationRounds <= 0;
        RemainingRounds = data.DurationRounds;
    }

    public bool Tick()
    {
        if (permanent) return false;
        RemainingRounds--;
        return RemainingRounds <= 0;
    }
}