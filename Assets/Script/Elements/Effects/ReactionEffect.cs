using UnityEngine;

public abstract class ReactionEffect : ScriptableObject
{
    public abstract void Apply(in ReactionContext ctx);
}

[CreateAssetMenu(fileName = "DamageEffect", menuName = "CyberCardGame/Effects/Damage")]
public class DamageEffect : ReactionEffect
{
    [SerializeField] private float damageAmount = 25f;

    public override void Apply(in ReactionContext ctx)
    {
        if (ctx.Target != null && ctx.Target.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(damageAmount);
    }
}

[CreateAssetMenu(fileName = "SlowEffect", menuName = "CyberCardGame/Effects/Slow")]
public class SlowEffect : ReactionEffect
{
    [SerializeField, Range(0.1f, 0.9f)] private float slowPercent = 0.5f;
    [SerializeField] private float duration = 3f;

    public override void Apply(in ReactionContext ctx)
    {
        if (ctx.Target != null && ctx.Target.TryGetComponent<ISlowable>(out var slowable))
            slowable.ApplySlow(slowPercent, duration);
    }
}


[CreateAssetMenu(fileName = "KnockbackEffect", menuName = "CyberCardGame/Effects/Knockback")]
public class KnockbackEffect : ReactionEffect
{
    [SerializeField] private float distance = 2f;

    public override void Apply(in ReactionContext ctx)
    {
        if (ctx.Target != null && ctx.Target.TryGetComponent<IKnockbackable>(out var knockbackable))
            knockbackable.Knockback(ctx.Direction, distance);
    }
}