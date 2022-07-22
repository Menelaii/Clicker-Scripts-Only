using UnityEngine;

[CreateAssetMenu(fileName = "New Accumulator", menuName = "Custom/SpellElements/Accumulator")]
public class AccumulatorContainer : SpellElementContainer
{
    public Accumulator SpellElement;

    public override SpellElement Item => SpellElement;
}
