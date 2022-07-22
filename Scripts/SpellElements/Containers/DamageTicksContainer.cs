using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Ticks", menuName = "Custom/SpellElements/DamageTicks")]
public class DamageTicksContainer : SpellElementContainer
{
    public DamageTicks SpellElement;

    public override SpellElement Item => SpellElement;
}
