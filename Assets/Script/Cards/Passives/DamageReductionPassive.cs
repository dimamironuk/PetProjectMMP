using UnityEngine;

[CreateAssetMenu(menuName = "CyberCardGame/Passives/Damage Reduction")]
public class DamageReductionPassive : PassiveEffect
{
    [SerializeField, Range(0f, 0.9f)] private float reduction = 0.25f;

    public override float ModifyIncomingDamage(float amount) => amount * (1f - reduction);
}