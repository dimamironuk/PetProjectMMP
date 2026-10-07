using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "KnockbackEffect", menuName = "CyberCardGame/Effects/Knockback")]
public class KnockbackEffect : ReactionEffect
{
    public float distance = 2f;

    public override void Apply(GameObject target, Vector3 position, DiContainer container)
    {
        if (target.TryGetComponent<UnitMovement>(out var movement))
        {
            movement.PushBack(distance);
        }
    }
}