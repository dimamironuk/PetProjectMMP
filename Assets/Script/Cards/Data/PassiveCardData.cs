using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassiveCard", menuName = "CyberCardGame/Cards/Passive Card")]
public class PassiveCardData : CardData
{
    [SerializeField, Min(0)] private int durationRounds;
    [SerializeField] private List<PassiveEffect> effects = new();

    public int DurationRounds => durationRounds;
    public IReadOnlyList<PassiveEffect> Effects => effects;
}