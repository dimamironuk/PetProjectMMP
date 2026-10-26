using UnityEngine;

public abstract class CardData : ScriptableObject
{
    [SerializeField] private string cardName;
    [SerializeField] private Sprite icon;
    [SerializeField] private CardCost cost;
    [SerializeField, TextArea] private string description;

    public string CardName => cardName;
    public Sprite Icon => icon;
    public CardCost Cost => cost;
    public string Description => description;
}