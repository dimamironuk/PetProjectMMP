using System.Collections.Generic;
using UnityEngine;

public enum CardTargetType
{
    SingleAlly,
    AllAllies
}

[CreateAssetMenu(fileName = "NewActionCard", menuName = "CyberCardGame/Cards/Action Card")]
public class ActionCardData : CardData
{
    [SerializeField] private CardTargetType targetType = CardTargetType.SingleAlly;
    [SerializeField] private List<ReactionEffect> effects = new();

    public CardTargetType TargetType => targetType;
    public IReadOnlyList<ReactionEffect> Effects => effects;
}