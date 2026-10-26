using System;
using System.Collections.Generic;

public interface ITokenPool
{
    int Count { get; }
    IReadOnlyList<ElementData> Tokens { get; }
    event Action Changed;

    int CountOf(ElementData element);
    bool CanPay(CardCost cost);
    bool TryPay(CardCost cost);
    void StartRound();
}