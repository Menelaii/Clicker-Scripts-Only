using System;
using UnityEngine;

[Serializable]
public class UpgradingSettings
{
    [Range(0, 6)] 
    public float UpgradingPercent;
    public int Coefficient = 1;
    public UpgradingAction UpgradingAction;
    public int MaxValue;
    public int MinValue;
}
