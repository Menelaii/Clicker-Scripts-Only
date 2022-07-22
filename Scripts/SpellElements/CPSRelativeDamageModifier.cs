using UnityEngine;

[System.Serializable]
public class CPSRelativeDamageModifier : SpellElement
{
    [SerializeField] private UpgradableStatInt _percentage;
    [SerializeField] private int _maxRewardedCPS;
    [SerializeField] private int _minRewardedCPS;

    public override void Apply()
    {
        int cps = SpellArgsProvider.Instance.AttackListener.ClicksPerSecond;
        int rewardedClicks = Mathf.Clamp((cps - _minRewardedCPS) + 1, 0, _maxRewardedCPS - _minRewardedCPS);
        int damage = Mathf.CeilToInt((Owner.Damage * _percentage.Value / 100f) * rewardedClicks);

        Owner.ModifyDamage(damage);
    }

    public override void Upgrade()
    {
        _percentage.Upgrade();
    }
}
