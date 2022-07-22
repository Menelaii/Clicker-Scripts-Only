using System;
using UnityEngine;

[Serializable]
public class RelativeStat
{
    [SerializeField] private UpgradableStatInt _stat;
    [SerializeField] private RelationType _relation;
    [SerializeField] private RelativeStatType _relativeTo;

    public UpgradableStatInt Stat => _stat;

    public int GetValue(Combatant target, Combatant owner)
    {
        return _relation == RelationType.Percentage
            ? SpellArgsProvider.Instance.GetRelativeValue(_stat.Value, _relativeTo, target, owner)
            : _stat.Value;
    }

    public void Upgrade()
    {
        _stat.Upgrade();
    }

    private enum RelationType
    {
        None,
        Percentage
    }
}
