using UnityEngine;
using Zenject;

public abstract class ReactionEffect : ScriptableObject
{
    public abstract void Apply(GameObject target, Vector3 position, DiContainer container);
}