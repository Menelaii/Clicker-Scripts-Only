using UnityEngine;

[CreateAssetMenu(fileName = "New Healing", menuName = "Custom/SpellElements/Healing")]
public class HealingContainer : SpellElementContainer
{
    public Healing Effect;

    public override SpellElement Item => Effect;
}
