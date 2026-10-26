using System.Collections.Generic;
using UnityEngine;

public class PlayerBoard : MonoBehaviour, IBoard
{
    [SerializeField] private CharacterSlot[] slots;

    public IReadOnlyList<CharacterSlot> Slots => slots;

    public IEnumerable<CharacterUnit> Characters
    {
        get
        {
            foreach (var slot in slots)
            {
                if (slot == null || slot.Unit == null || !slot.Unit.IsAlive) continue;
                yield return slot.Unit;
            }
        }
    }

    private void Awake()
    {
        if (HasMissingSlots()) slots = GetComponentsInChildren<CharacterSlot>(true);

        if (slots.Length == 0)
            Debug.LogError("PlayerBoard: no CharacterSlot found in children", this);
    }

    public bool TryDeploy(CharacterCardData data)
        => TryGetFreeSlot(out var slot) && slot.Place(data) != null;

    public bool HasSlot(CharacterSlot slot)
    {
        if (slot == null || slots == null) return false;

        foreach (var candidate in slots)
            if (candidate == slot) return true;

        return false;
    }

    public bool HasUnit(CharacterUnit unit)
    {
        if (unit == null || slots == null) return false;

        foreach (var slot in slots)
            if (slot != null && slot.Unit == unit) return true;

        return false;
    }
    public bool TryGetFreeSlot(out CharacterSlot slot)
    {
        if (slots == null)
        {
            slot = null;
            return false;
        }

        foreach (var candidate in slots)
        {
            if (candidate == null || !candidate.IsEmpty) continue;
            slot = candidate;
            return true;
        }

        slot = null;
        return false;
    }

    private bool HasMissingSlots()
    {
        if (slots == null || slots.Length == 0) return true;

        foreach (var slot in slots)
            if (slot == null) return true;

        return false;
    }
}