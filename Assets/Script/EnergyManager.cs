using System;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public int maxRam = 10;
    public float currentRam = 5f;
    public float regenRate = 1f;

    public event Action<int> OnRamChanged;

    private void Update()
    {
        if (currentRam < maxRam)
        {
            currentRam = Mathf.Min(currentRam + regenRate * Time.deltaTime, maxRam);
            OnRamChanged?.Invoke(Mathf.FloorToInt(currentRam));
        }
    }

    public bool ConsumeRam(int amount)
    {
        if (currentRam >= amount)
        {
            currentRam -= amount;
            OnRamChanged?.Invoke(Mathf.FloorToInt(currentRam));
            return true;
        }
        return false;
    }
}