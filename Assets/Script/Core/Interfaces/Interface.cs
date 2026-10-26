using UnityEngine;

public interface IDamageable { void TakeDamage(float amount); }
public interface IHealable { void Heal(float amount); }
public interface ISlowable { void ApplySlow(float percent, float duration); }
public interface IKnockbackable { void Knockback(Vector3 direction, float distance); }

public interface IDeck
{
    CardData Draw();
    void Discard(CardData card);
}

public interface IHand
{
    bool HasSpace { get; }
    void Add(CardData card);
    void Remove(CardUI card);
}