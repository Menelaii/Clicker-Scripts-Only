using System.Linq;
using UnityEngine;

[System.Serializable]
public class Corpus : BodyPart
{
    [SerializeField] private BodyPartStatInt _maxHealth;
    [SerializeField] private BodyPartStatInt _healthRegeneration;
    [SerializeField] private BodyPartStatInt _armor;

    public int MaxHealth => _maxHealth.StatInt.Value;
    public int HealthRegeneration => _healthRegeneration.StatInt.Value;
    public int Armor => _armor.StatInt.Value;

    public override void SetStatNames()
    {
        base.SetStatNames();
        _maxHealth.SetStatName(nameof(_maxHealth));
        _healthRegeneration.SetStatName(nameof(_healthRegeneration));
        _armor.SetStatName(nameof(_armor));
    }

    public override string GetStatsLine()
    {
        return $"Health: {_maxHealth}\nHealth Regen: {_healthRegeneration}\nArmor: {_armor}";
    }

    protected override BodyPartStat[] GetStats()
    {
        BodyPartStat[] baseStats = base.GetStats();
        BodyPartStat[] stats = new BodyPartStat[]
        {
            _maxHealth,
            _healthRegeneration,
            _armor
        };


        return baseStats.Union(stats).ToArray();
    }
}
