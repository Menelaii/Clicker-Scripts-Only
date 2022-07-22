using UnityEngine;

[System.Serializable]
public abstract class BodyPartStat
{
    [SerializeField] private int _upgradingPerLevel = 1;

    [HideInInspector] [SerializeField]
    private BodyPartStatType _statName;

    public BodyPartStatType Name => _statName;

    public abstract UpgradableStat Stat { get; }

    public void SetStatName(string fieldName)
    {
        _statName = BodyPartStatMatcher.GetMatch(fieldName);
    }

    public void Upgrade(int bodypartLevel)
    {
        if (_upgradingPerLevel == 0)
            return;

        if (bodypartLevel % _upgradingPerLevel != 0)
            return;

        Stat.Upgrade();
    }
}
