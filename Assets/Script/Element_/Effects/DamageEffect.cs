using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "CyberCardGame/Effects/Damage")]
public class DamageEffect : ReactionEffect
{
    public float damageAmount = 25f;

    public override void Apply(GameObject target, Vector3 position, DiContainer container)
    {
        if (target.TryGetComponent<UnitHealth>(out var health))
        {
            health.TakeDamage(damageAmount);
        }
    }
}