public class HealthRegeneration : Regeneration
{
    private Health _owner;

    public HealthRegeneration(Health owner, int pointsPerSecond) : base(pointsPerSecond)
    {
        _owner = owner;
    }

    public override void OnBodyChanged(Body body)
    {
        PointsPerSecond = body.Corpus.HealthRegeneration;
        base.OnBodyChanged(body);
    }

    protected override void Regenerate()
    {
        _owner.Heal(PointsPerSecond);
    }
}
