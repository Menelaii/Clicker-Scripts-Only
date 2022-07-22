using System.Linq;
using UnityEngine;

[System.Serializable]
public class Legs : BodyPart
{
    [SerializeField] private BodyPartStatInt _critChance;
    [SerializeField] private BodyPartStatInt _critDamageInPercent;
    [SerializeField] private BodyPartStatInt _dodgeChance;

    public int CritChance => _critChance.StatInt.Value;
    public int CritDamageInPercent => _critDamageInPercent.StatInt.Value;
    public int DodgeChance => _dodgeChance.StatInt.Value;


    public override void SetStatNames()
    {
        base.SetStatNames();
        _critChance.SetStatName(nameof(_critChance));
        _critDamageInPercent.SetStatName(nameof(_critDamageInPercent));
        _dodgeChance.SetStatName(nameof(_dodgeChance));
    }

    public override string GetStatsLine()
    {
        return $"Crit chance: {_critChance}%\nCrit Damage: {_critDamageInPercent}%\nDodge chance: {_dodgeChance}%";
    }

    protected override BodyPartStat[] GetStats()
    {
        BodyPartStat[] baseStats = base.GetStats();
        BodyPartStat[] stats = new BodyPartStat[]
        {
            _critChance,
            _critDamageInPercent,
            _dodgeChance
        };


        return baseStats.Union(stats).ToArray();
    }
}
