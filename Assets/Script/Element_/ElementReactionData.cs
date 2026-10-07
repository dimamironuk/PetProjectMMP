using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "NewReaction", menuName = "CyberCardGame/Data/Element Reaction")]
public class ElementReactionData : ScriptableObject
{
    public string reactionName;
    public ElementData elementA;
    public ElementData elementB;
    public GameObject vfxPrefab;
    public List<ReactionEffect> effects = new List<ReactionEffect>();

    public bool Matches(ElementData a, ElementData b)
    {
        return (elementA == a && elementB == b) || (elementA == b && elementB == a);
    }

    public void TriggerReaction(GameObject target, Vector3 position, DiContainer container)
    {
        if (vfxPrefab != null && container != null)
        {
            container.InstantiatePrefab(vfxPrefab, position, Quaternion.identity, null);
        }

        foreach (var effect in effects)
        {
            if (effect != null) effect.Apply(target, position, container);
        }
    }
}