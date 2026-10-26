using System;
using UnityEngine;

public class AbilityService
{
    private readonly ITokenPool tokens;

    public event Action<CharacterUnit, AbilityData> AbilityUsed;

    public AbilityService(ITokenPool tokens)
    {
        this.tokens = tokens;
    }

    public bool TryUse(CharacterUnit source, AbilityData ability, CharacterUnit target)
    {
        if (ability == null || source == null || target == null || !source.IsAlive || !target.IsAlive) return false;
        if (!tokens.TryPay(ability.Cost)) return false;

        ApplyTo(target, ability);
        AbilityUsed?.Invoke(source, ability);
        return true;
    }

    private static void ApplyTo(CharacterUnit target, AbilityData ability)
    {
        var context = new ReactionContext(target.gameObject, target.transform.position, target.transform.forward);

        foreach (var effect in ability.Effects)
            if (effect != null) effect.Apply(context);

        if (!target.IsAlive || ability.AppliedElement == null) return;

        if (target.TryGetComponent<StatusReceiver>(out var status))
            status.ApplyElement(ability.AppliedElement);
    }
}