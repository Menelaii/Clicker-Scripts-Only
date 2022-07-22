using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Custom/Enemy")]
public class EnemyStaticData : ScriptableObject
{
    public int MaxHealth;
    public int Damage;
    public int CritDamageInPercent;
    public int CritChance;
    public int Armor;
    public int DodgeChance;
    public float AttackCooldown;
    public float AttackCooldownSpread;
    public int StatIncreasePercent;
    public SpellContainer[] AttackModifierContainers;
    public int ManaRegen;
    public int MaxMana;
    public Loot Loot;
    public Combatant Prefab;

    [HideInInspector]
    public Spell[] AttackModifiers;

    private void OnValidate()
    {
        if (AttackModifierContainers == null)
            return;

        AttackModifiers = GetAttackModifiers();
    }

    private Spell[] GetAttackModifiers()
    {
        Spell[] abilities = new Spell[AttackModifierContainers.Length];
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = AttackModifierContainers[i]?.Item;
        }

        return abilities;
    }
}
