using System.Collections.Generic;

public class SpellUser : IBodyChangesHandler
{
    public Mana Mana { get; private set; }
    public ManaRegeneration ManaRegeneration { get; private set; }

    public SpellUser(Mana mana, int manaRegenerationPerSecond)
    {
        Mana = mana;

        ManaRegeneration = new ManaRegeneration(mana, manaRegenerationPerSecond);
        ManaRegeneration.StartRegeneration();
    }

    public void TryUse(IReadOnlyList<Spell> spells)
    {
        if (spells == null)
            return;

        foreach (var spell in spells)
        {
            TryUse(spell);
        }
    }

    public bool TryUse(Spell spell)
    {
        if (Mana.IsEnoughFor(spell) == false || (spell.NeedEnemyToUse && SpellArgsProvider.Instance.Enemy == null))
            return false;
        
        spell.Apply();

        Mana.Use(spell.ManaCostInPercent);

        return true;
    }

    public void OnBodyChanged(Body body)
    {
        Mana.OnBodyChanged(body);
        ManaRegeneration.OnBodyChanged(body);
    }
}
