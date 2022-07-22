using UnityEngine;

[CreateAssetMenu(fileName = "New Single Damage", menuName = "Custom/SpellElements/SingleDamage")]
public class SingleDamageContainer : SpellElementContainer
{
    public SingleDamage SpellElement;

    public override SpellElement Item => SpellElement;
}
