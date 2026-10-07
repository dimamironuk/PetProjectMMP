using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SlowEffect", menuName = "CyberCardGame/Effects/Slow")]
public class SlowEffect : ReactionEffect
{
    [Range(0.1f, 0.9f)] public float slowPercent = 0.5f;
    public float duration = 3f;

    public override void Apply(GameObject target, Vector3 position, DiContainer container)
    {
        if (target.TryGetComponent<UnitMovement>(out var movement))
        {
            movement.ApplySlow(slowPercent, duration);
        }
    }
}