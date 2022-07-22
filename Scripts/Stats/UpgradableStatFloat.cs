using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class UpgradableStatFloat : UpgradableStat
{
    private const int DIGITS_AFTER_POINT = 2;

    [FormerlySerializedAs("_currentValue")]
    [SerializeField] private float _value;

    public float Value => _value;

    public override void Spread(int maxSpreadPercent)
    {
         _value *= Random.Range(-maxSpreadPercent, maxSpreadPercent) / 100f;
        ClampAndRound();
    }

    public override void Upgrade()
    {
        AddOrSubstractPercentage(UpgradingSettings.UpgradingPercent, UpgradingSettings.UpgradingAction);

        _value *= UpgradingSettings.Coefficient;

        ClampAndRound();
    }

    public override void ChangeValue(float percentage, UpgradingAction upgradingAction)
    {
        AddOrSubstractPercentage(percentage, upgradingAction);
        ClampAndRound();
    }

    public override string ToString() => _value.ToString();

    private void ClampAndRound()
    {
        _value = Mathf.Clamp(_value, UpgradingSettings.MinValue, UpgradingSettings.MaxValue);
        _value = (float)Math.Round(_value, DIGITS_AFTER_POINT);
    }

    private void AddOrSubstractPercentage(float upgradingPercent, UpgradingAction upgradingAction)
    {
        float delta = _value * upgradingPercent;
        _value = upgradingAction == UpgradingAction.Increasing ? _value + delta : _value - delta;
    }
}
