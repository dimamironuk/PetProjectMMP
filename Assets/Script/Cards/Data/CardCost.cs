using System;
using UnityEngine;

[Serializable]
public struct CardCost
{
    public ElementData element;
    [Min(0)] public int elementAmount;
    [Min(0)] public int genericAmount;

    public int Total => elementAmount + genericAmount;
}