using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "CyberCardGame/Cards/Character Card")]
public class CharacterCardData : CardData
{
    [SerializeField] private ElementData element;
    [SerializeField, Min(1)] private int maxHp = 10;
    [SerializeField] private List<AbilityData> abilities = new();

    public ElementData Element => element;
    public int MaxHp => maxHp;
    public IReadOnlyList<AbilityData> Abilities => abilities;
}