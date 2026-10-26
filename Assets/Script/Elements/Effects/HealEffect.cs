using UnityEngine;

[CreateAssetMenu(menuName = "CyberCardGame/Effects/Heal")]
public class HealEffect : ReactionEffect
{
    [SerializeField] private float healAmount = 3f;

    public override void Apply(in ReactionContext ctx)
    {
        if (ctx.Target != null && ctx.Target.TryGetComponent<IHealable>(out var healable))
            healable.Heal(healAmount);
    }
}