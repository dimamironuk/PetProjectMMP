using UnityEngine;

public abstract class PassiveEffect : ScriptableObject
{
    public virtual void OnRoundStart(CharacterUnit owner) { }
    public virtual float ModifyIncomingDamage(float amount) => amount;
}