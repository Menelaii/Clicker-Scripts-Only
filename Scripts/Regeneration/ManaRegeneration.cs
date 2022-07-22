public class ManaRegeneration : Regeneration
{
    private Mana _owner;

    public ManaRegeneration(Mana owner, int pointsPerSecond) : base(pointsPerSecond)
    {
        _owner = owner;
    }

    public override void OnBodyChanged(Body body)
    {
        PointsPerSecond = body.Head.ManaRegeneration;
        base.OnBodyChanged(body);
    }

    protected override void Regenerate()
    {
        _owner.Add(PointsPerSecond);
    }
}
