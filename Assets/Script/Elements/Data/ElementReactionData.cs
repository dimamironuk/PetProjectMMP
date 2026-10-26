using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewReaction", menuName = "CyberCardGame/Dynamic/Element Reaction")]
public class ElementReactionData : ScriptableObject
{
    [Header("Reaction Settings")]
    [SerializeField] private string reactionName;
    [SerializeField] private ElementData elementA;
    [SerializeField] private ElementData elementB;

    [Header("Effects")]
    [SerializeField] private List<ReactionEffect> effects = new();

    public string ReactionName => reactionName;

    public bool Matches(ElementData x, ElementData y)
    {
        return (elementA == x && elementB == y) ||
               (elementA == y && elementB == x);
    }

    public void TriggerReaction(in ReactionContext ctx)
    {
        foreach (var effect in effects)
        {
            if (effect != null) effect.Apply(ctx);
        }
    }
}