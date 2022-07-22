using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Cooldown Increase", menuName = "Custom/SpellElements/AttackCooldownIncrease")]
public class AttackCooldownIncreaseContainer : SpellElementContainer
{
    public AttackCooldownIncrease SpellElement;

    public override SpellElement Item => SpellElement;
}
