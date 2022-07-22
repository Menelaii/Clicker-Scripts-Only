using System;
using UnityEngine;

[Serializable]
public abstract class SpellElement
{
    [SerializeField] private TargetType _owner;
    [SerializeField] private TargetType _target;

    public Combatant Owner => SpellArgsProvider.Instance.GetCombatant(_owner);
    public Combatant Target => SpellArgsProvider.Instance.GetCombatant(_target);
    public TargetType TargetName => _target;

    public abstract void Upgrade();

    public abstract void Apply();
}