using UnityEngine;

[CreateAssetMenu(fileName = "NewElement", menuName = "CyberCardGame/Dynamic/Element Data")]
public class ElementData : ScriptableObject
{
    [Header("Element Settings")]
    public string elementName;     
    public Color elementColor = Color.cyan;
    public Sprite elementIcon;       
    [TextArea] public string description;
}