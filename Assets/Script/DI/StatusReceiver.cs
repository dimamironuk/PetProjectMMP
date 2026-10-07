using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StatusReceiver : MonoBehaviour
{
    [Inject] private ReactionDatabase reactionDatabase;
    [Inject] private DiContainer container;

    public List<ElementData> activeElements = new List<ElementData>();

    public void ApplyElement(ElementData newElement)
    {
        if (newElement == null) return;

        foreach (var existing in new List<ElementData>(activeElements))
        {
            var reaction = reactionDatabase.GetReaction(existing, newElement);
            if (reaction != null)
            {
                activeElements.Remove(existing);
                reaction.TriggerReaction(gameObject, transform.position, container);
                return;
            }
        }

        if (!activeElements.Contains(newElement))
            activeElements.Add(newElement);
    }
}