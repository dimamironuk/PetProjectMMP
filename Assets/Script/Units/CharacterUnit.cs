using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CharacterUnit : MonoBehaviour, IDamageable, IHealable, ISlowable, IKnockbackable
{
    [SerializeField, Min(0)] private int maxPassives = 2;

    private readonly List<ActivePassive> passives = new();

    public CharacterCardData Data { get; private set; }
    public float CurrentHp { get; private set; }
    public float MaxHp => Data != null ? Data.MaxHp : 0f;
    public bool IsAlive => CurrentHp > 0f;
    public bool CanAttachPassive => IsAlive && passives.Count < maxPassives;
    public IReadOnlyList<ActivePassive> Passives => passives;

    public event Action<CharacterCardData> Initialized;
    public event Action<float, float> HpChanged;
    public event Action PassivesChanged;
    public event Action<CharacterUnit> Died;

    private float slowUntil;
    private Tweener knockbackTween;

    public bool IsSlowed => Time.time < slowUntil;

    public void Initialize(CharacterCardData data)
    {
        if (data == null) return;

        Data = data;
        CurrentHp = data.MaxHp;
        slowUntil = 0f;

        Initialized?.Invoke(data);
        HpChanged?.Invoke(CurrentHp, MaxHp);
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive || amount <= 0f) return;

        foreach (var passive in passives)
        {
            if (passive?.Data?.Effects == null) continue;
            foreach (var effect in passive.Data.Effects)
            {
                if (effect == null) continue;
                amount = effect.ModifyIncomingDamage(amount);
            }
        }

        SetHp(CurrentHp - amount);

        if (!IsAlive) Died?.Invoke(this);
    }

    public void Heal(float amount)
    {
        if (!IsAlive || amount <= 0f) return;
        SetHp(CurrentHp + amount);
    }

    public bool TryAttachPassive(PassiveCardData data)
    {
        if (!CanAttachPassive) return false;

        passives.Add(new ActivePassive(data));
        PassivesChanged?.Invoke();
        return true;
    }

    public void ApplySlow(float percent, float duration)
    {
        if (!IsAlive || duration <= 0f) return;
        slowUntil = Mathf.Max(slowUntil, Time.time + duration);
    }

    public void Knockback(Vector3 direction, float distance)
    {
        if (!IsAlive || distance <= 0f) return;

        Vector3 offset = direction.sqrMagnitude > 0.001f ? direction.normalized * distance : Vector3.right * distance;
        knockbackTween?.Kill(true);

        if (transform is RectTransform rect)
            knockbackTween = rect.DOPunchAnchorPos(offset, 0.35f, 8);
        else
            knockbackTween = transform.DOPunchPosition(offset, 0.35f, 8);
    }

    public void OnRoundStart()
    {
        if (!IsAlive) return;

        bool removedAny = false;

        for (int i = passives.Count - 1; i >= 0; i--)
        {
            var passive = passives[i];
            if (passive?.Data?.Effects == null) continue;

            foreach (var effect in passive.Data.Effects)
            {
                if (effect == null) continue;
                effect.OnRoundStart(this);
            }

            if (!passive.Tick()) continue;

            passives.RemoveAt(i);
            removedAny = true;
        }

        if (removedAny) PassivesChanged?.Invoke();
    }

    private void SetHp(float value)
    {
        CurrentHp = Mathf.Clamp(value, 0f, MaxHp);
        HpChanged?.Invoke(CurrentHp, MaxHp);
    }

    private void OnDestroy()
    {
        knockbackTween?.Kill();
        Died = null;
        HpChanged = null;
        Initialized = null;
        PassivesChanged = null;
    }
}