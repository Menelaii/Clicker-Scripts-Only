using UnityEngine;

[CreateAssetMenu(fileName = "New Legs", menuName = "Custom/BodyParts/Legs")]
public class LegsContainer : BodyPartContainer
{
    public Legs Legs;

    public override BodyPart Item => Legs;
}
