using System;
using UnityEngine;

[Serializable]
public class AttackCooldownIncrease : SpellElement
{
    [SerializeField] private UpgradableStatInt _percent;
    [SerializeField] private UpgradableStatFloat _duration;

    public override void Upgrade()
    {
        _percent.Upgrade();
        _duration.Upgrade();
    }

    public override void Apply()
    {
        Target.AutoAttacker.IncreaseAttackCooldown(_percent.Value);

        Action action = () => Target?.AutoAttacker.IncreaseAttackCooldown(-_percent.Value);
        CoroutineMaster.Instance.InvokeAfterDelay(action, _duration.Value);
    }
}