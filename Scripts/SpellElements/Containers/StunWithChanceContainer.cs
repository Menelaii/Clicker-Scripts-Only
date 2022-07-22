using UnityEngine;

[CreateAssetMenu(fileName = "New Stun", menuName = "Custom/SpellElements/Stun")]
public class StunWithChanceContainer : SpellElementContainer
{
    public StunWithChance SpellElement;

    public override SpellElement Item => SpellElement;
}
