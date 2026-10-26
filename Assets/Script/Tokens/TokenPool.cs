using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenPool : ITokenPool
{
    private readonly TokenSettings settings;
    private readonly List<ElementData> tokens = new();
    private System.Random rng = new();

    public TokenPool(TokenSettings settings)
    {
        this.settings = settings;
    }

    public int Count => tokens.Count;
    public IReadOnlyList<ElementData> Tokens => tokens;
    public event Action Changed;

    public void Initialize(int seed) => rng = new System.Random(seed);

    public void StartRound()
    {
        if (settings.Elements.Count == 0) return;

        if (!settings.CarryOver) tokens.Clear();

        int amount = Mathf.Min(settings.TokensPerRound, settings.MaxTokens - tokens.Count);

        for (int i = 0; i < amount; i++)
            tokens.Add(settings.Elements[rng.Next(settings.Elements.Count)]);

        tokens.Sort((a, b) => settings.IndexOf(a).CompareTo(settings.IndexOf(b)));
        Changed?.Invoke();
    }

    public int CountOf(ElementData element)
    {
        int count = 0;
        foreach (var token in tokens)
            if (token == element) count++;
        return count;
    }

    public bool CanPay(CardCost cost)
    {
        if (cost.Total > tokens.Count) return false;
        if (cost.elementAmount <= 0) return true;
        return cost.element != null && CountOf(cost.element) >= cost.elementAmount;
    }

    public bool TryPay(CardCost cost)
    {
        if (!CanPay(cost)) return false;

        RemoveOfType(cost.element, cost.elementAmount);
        RemoveGeneric(cost.genericAmount);
        Changed?.Invoke();
        return true;
    }

    private void RemoveOfType(ElementData element, int amount)
    {
        for (int i = tokens.Count - 1; i >= 0 && amount > 0; i--)
        {
            if (tokens[i] != element) continue;
            tokens.RemoveAt(i);
            amount--;
        }
    }

    private void RemoveGeneric(int amount)
    {
        while (amount-- > 0)
            tokens.Remove(FindMostCommon());
    }

    private ElementData FindMostCommon()
    {
        ElementData best = null;
        int bestCount = 0;

        foreach (var candidate in tokens)
        {
            int count = CountOf(candidate);
            if (count <= bestCount) continue;
            best = candidate;
            bestCount = count;
        }

        return best;
    }
}