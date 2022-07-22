using UnityEngine;

[System.Serializable]
public abstract class UpgradableStat
{
    [SerializeField] protected UpgradingSettings UpgradingSettings;

    public void Increase(float percentage) => ChangeValue(percentage, UpgradingAction.Increasing);
    public void Decrease(float percentage) => ChangeValue(percentage, UpgradingAction.Decreasing);

    public abstract void Upgrade();
    public abstract void Spread(int maxSpreadPercent);
    public abstract void ChangeValue(float percentage, UpgradingAction upgradingAction);
}
