using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class UpgradableStatInt : UpgradableStat
{
    [FormerlySerializedAs("_currentValue")]
    [SerializeField] private int _value;

    public int Value => _value;

    public override void Spread(int maxSpreadPercent)
    {
        _value += Mathf.RoundToInt(_value * Random.Range(-maxSpreadPercent, maxSpreadPercent) / 100f);
        ClampValue();
    }

    public override void Upgrade()
    {
        AddOrSubstractPercentage(UpgradingSettings.UpgradingPercent, UpgradingSettings.UpgradingAction);

        _value *= UpgradingSettings.Coefficient;

        ClampValue();
    }

    public override void ChangeValue(float percentage, UpgradingAction upgradingAction)
    {
        AddOrSubstractPercentage(percentage, upgradingAction);
        ClampValue();
    }

    private void AddOrSubstractPercentage(float percentage, UpgradingAction upgradingAction)
    {
        int delta = Mathf.RoundToInt(_value * percentage);
        _value = upgradingAction == UpgradingAction.Increasing
            ? _value + delta
            : _value - delta;
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    private void ClampValue() => 
        _value = Mathf.Clamp(_value, UpgradingSettings.MinValue, UpgradingSettings.MaxValue);
}
