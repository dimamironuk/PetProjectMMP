using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "CyberCardGame/Cards/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public ElementData element;
    public int energyCost = 1;
    public Sprite cardIcon;
    [TextArea] public string description;
}