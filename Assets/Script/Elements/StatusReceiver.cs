using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StatusReceiver : MonoBehaviour
{
    [Inject] private ReactionDatabase reactionDatabase;

    private readonly List<ElementData> active = new();
    public IReadOnlyList<ElementData> ActiveElements => active;

    public void ApplyElement(ElementData incoming)
    {
        if (incoming == null || reactionDatabase == null) return;

        for (int i = active.Count - 1; i >= 0; i--)  
        {
            var reaction = reactionDatabase.GetReaction(active[i], incoming);
            if (reaction == null) continue;

            active.RemoveAt(i);
            reaction.TriggerReaction(new ReactionContext(gameObject, transform.position, transform.forward));
            return;
        }

        if (!active.Contains(incoming)) active.Add(incoming);
    }
}