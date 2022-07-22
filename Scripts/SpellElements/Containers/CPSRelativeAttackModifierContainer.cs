using UnityEngine;

[CreateAssetMenu(fileName = "New CPS AM", menuName = "Custom/SpellElements/CPSAttackModifier")]
public class CPSRelativeAttackModifierContainer : SpellElementContainer
{
    public CPSRelativeDamageModifier SpellElement;

    public override SpellElement Item => SpellElement;
}
