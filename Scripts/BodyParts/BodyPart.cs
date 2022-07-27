using System;
using UnityEngine;

[Serializable]
public abstract class BodyPart
{
    private static readonly int DEFAULT_MAX_LEVEL = 100;

    [SerializeField] private SetId _setId;
    [SerializeField] private int _level;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _label;
    [SerializeField] private BodyPartStatInt _price;
    [SerializeField] private BodyPartStatInt _upgradePrice;

    private BodyPartStat[] _stats;

    public BodyPartStat[] Stats { get => _stats ?? (_stats = GetStats()); }
    public SetId SetId => _setId;
    public Sprite Icon => _icon;
    public string Label => _label;
    public int Price => _price.StatInt.Value;
    public int UpgradePrice => _upgradePrice.StatInt.Value;

    public UpgradableStat GetStat(BodyPartStatType targetStatName)
    {
        foreach (var bodyPartStat in Stats)
        {
            if (bodyPartStat.Name == targetStatName)
                return bodyPartStat.Stat;
        }

        throw new ArgumentException();
    }

    public virtual void Upgrade()
    {
        if (_level + 1 > DEFAULT_MAX_LEVEL)
            return;

        _level++;
        foreach (var bodyPartStat in Stats)
        {
            bodyPartStat.Upgrade(_level);
        }
    }

    public virtual void SetStatNames()
    {
        _price.SetStatName(nameof(_price));
        _upgradePrice.SetStatName(nameof(_upgradePrice));
    }

    public void OnDrop(int upgradesPerWave, int globalWaveIndex, int maxSpreadPercent)
    {
        int upgradesCount = globalWaveIndex * upgradesPerWave;
        for (int i = 0; i < upgradesCount; i++)
        {
            Upgrade();
        }

        SpreadStats(maxSpreadPercent);
    }

    public void SpreadStats(int maxSpreadPercent)
    {
        foreach (var bodyPartStat in Stats)
        {
            bodyPartStat.Stat.Spread(maxSpreadPercent);
        }
    }

    public abstract string GetStatsLine();

    protected virtual BodyPartStat[] GetStats()
    {
        return new BodyPartStat[]
        {
            _price,
            _upgradePrice
        };
    }
}
