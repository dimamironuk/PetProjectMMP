using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TokenSettings", menuName = "CyberCardGame/Data/Token Settings")]
public class TokenSettings : ScriptableObject
{
    [SerializeField] private List<ElementData> elements = new();
    [SerializeField, Min(1)] private int tokensPerRound = 4;
    [SerializeField, Min(1)] private int maxTokens = 10;
    [SerializeField] private bool carryOver = true;

    public IReadOnlyList<ElementData> Elements => elements;
    public int TokensPerRound => tokensPerRound;
    public int MaxTokens => maxTokens;
    public bool CarryOver => carryOver;

    public int IndexOf(ElementData element) => elements.IndexOf(element);
}