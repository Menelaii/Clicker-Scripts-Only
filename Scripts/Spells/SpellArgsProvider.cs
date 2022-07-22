using System;
using UnityEngine;

public class SpellArgsProvider
{
    public Combatant Player { get; private set; }
    public Combatant Enemy { get; private set; }
    public GameFactory GameFactory { get; private set; }
    public AttackButtonListener AttackListener { get; private set; }

    public static SpellArgsProvider Instance;

    public SpellArgsProvider(Combatant player, Combatant enemy,
        GameFactory gameFactory, AttackButtonListener attackListener)
    {
        Player = player;
        Enemy = enemy;
        GameFactory = gameFactory;
        AttackListener = attackListener;

        Instance = Instance == null 
            ? this : throw new InvalidOperationException();
    }

    public void OnTargetChanged(Combatant enemy)
    {
        Enemy = enemy;
    }

    public Combatant GetCombatant(TargetType target)
    {
        return target == TargetType.Enemy ? Enemy : Player;
    }

    public int GetRelativeValue(int percentage, RelativeStatType stat, Combatant target, Combatant owner)
    {
        int relativeValue = stat switch
        {
            RelativeStatType.TargetMaxHealth => target.Health.MaxValue,
            RelativeStatType.TargetCurrentHealth => target.Health.Value,
            RelativeStatType.OwnerDamage => owner.Damage,
            _ => throw new NotImplementedException()
        };

        return Mathf.CeilToInt(relativeValue * (percentage / 100f));
    }
}
