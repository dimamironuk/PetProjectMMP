using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReactionDatabase", menuName = "CyberCardGame/Data/Reaction Database")]
public class ReactionDatabase : ScriptableObject
{
    public List<ElementReactionData> allReactions = new List<ElementReactionData>();

    public ElementReactionData GetReaction(ElementData elemA, ElementData elemB)
    {
        if (elemA == null || elemB == null) return null;

        foreach (var reaction in allReactions)
        {
            if (reaction != null && reaction.Matches(elemA, elemB))
                return reaction;
        }

        return null;
    }
}