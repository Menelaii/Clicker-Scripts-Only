using UnityEngine;

public abstract class Bonus : ScriptableObject
{
    public abstract void EnableOn(Body body);

    public abstract void DisableOn(Body body);

    protected bool IsTargetBodyPartWasReplacedIn(Body body, BodyPartType targetBP)
    {
        return body.LastReplacedPart != null
            && body.LastReplacedPart.GetType().Name == targetBP.ToString();
    }
}
