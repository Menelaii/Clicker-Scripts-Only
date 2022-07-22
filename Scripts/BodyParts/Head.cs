using UnityEngine;
using System.Linq;

[System.Serializable]
public class Head : BodyPartWithSpells
{
    [SerializeField] private BodyPartStatInt _maxMana;
    [SerializeField] private BodyPartStatInt _manaRegeneration;

    public int MaxMana => _maxMana.StatInt.Value;
    public int ManaRegeneration => _manaRegeneration.StatInt.Value;

    public override void SetStatNames()
    {
        base.SetStatNames();
        _maxMana.SetStatName(nameof(_maxMana));
        _manaRegeneration.SetStatName(nameof(_manaRegeneration));
    }

    public override string GetStatsLine()
    {
        return $"Mana: {_maxMana}\nMana regen: {_manaRegeneration}";
    }

    protected override BodyPartStat[] GetStats()
    {
        BodyPartStat[] baseStats = base.GetStats();
        BodyPartStat[] stats = new BodyPartStat[]
        {
            _maxMana,
            _manaRegeneration
        };


        return baseStats.Union(stats).ToArray();
    }
}
