using UnityEngine;

[CreateAssetMenu(menuName = "CyberCardGame/Passives/Regen")]
public class RegenPassive : PassiveEffect
{
    [SerializeField] private float healPerRound = 2f;

    public override void OnRoundStart(CharacterUnit owner) => owner.Heal(healPerRound);
}