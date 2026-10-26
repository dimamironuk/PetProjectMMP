using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "CyberCardGame/Cards/Ability")]
public class AbilityData : ScriptableObject
{
    [SerializeField] private string abilityName;
    [SerializeField] private Sprite icon;
    [SerializeField, TextArea] private string description;
    [SerializeField] private CardCost cost;
    [SerializeField] private ElementData appliedElement;
    [SerializeField] private List<ReactionEffect> effects = new();

    public string AbilityName => abilityName;
    public Sprite Icon => icon;
    public string Description => description;
    public CardCost Cost => cost;
    public ElementData AppliedElement => appliedElement;
    public IReadOnlyList<ReactionEffect> Effects => effects;
}