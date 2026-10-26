using UnityEngine;

public readonly struct ReactionContext
{
    public readonly GameObject Target;
    public readonly Vector3 Position;
    public readonly Vector3 Direction;

    public ReactionContext(GameObject target, Vector3 position, Vector3 direction)
    {
        Target = target; Position = position; Direction = direction;
    }
}