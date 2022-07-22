using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Changes", menuName = "Custom/Bonuses/StatChanges")]
public class StatChanges : Bonus
{
    [SerializeField] private BodyPartType _targetBodyPart;
    [SerializeField] private BodyPartStatType _targetStat;
    [SerializeField] private UpgradingAction _upgradingAction;

    [SerializeField] [Range(0, 6)] 
    private float _percentage;

    private void OnValidate()
    {
        StatChangesValidator.Validate(_targetBodyPart, _targetStat);
    }

    public override void DisableOn(Body body)
    {
        UpgradingAction degradingAction = _upgradingAction == UpgradingAction.Increasing
            ? UpgradingAction.Decreasing
            : UpgradingAction.Increasing;

        GetTargetFrom(body).ChangeValue(_percentage, degradingAction);
    }

    public override void EnableOn(Body body)
    {
        GetTargetFrom(body).ChangeValue(_percentage, _upgradingAction);
    }

    private UpgradableStat GetTargetFrom(Body body)
    {
        if(IsTargetBodyPartWasReplacedIn(body, _targetBodyPart))
            return body.LastReplacedPart.GetStat(_targetStat);

        return body
            .GetBodyPart(_targetBodyPart)
            .GetStat(_targetStat);
    }
}
