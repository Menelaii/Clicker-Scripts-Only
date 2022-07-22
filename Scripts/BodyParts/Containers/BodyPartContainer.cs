
public abstract class BodyPartContainer : Container<BodyPart>
{
    private void OnValidate()
    {
        Item.SetStatNames();
    }
}
