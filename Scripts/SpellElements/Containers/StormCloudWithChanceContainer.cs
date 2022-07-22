using UnityEngine;

[CreateAssetMenu(fileName = "New Storm Cloud", menuName = "Custom/SpellElements/StormCloud")]
public class StormCloudWithChanceContainer : SpellElementContainer
{
    public StormCloudWithChance SpellElement;

    public override SpellElement Item => SpellElement;
}
