using UnityEngine;

[System.Serializable]
public class StormCloudWithChance : SpellElement, ICreateSpellEntity
{
    [SerializeField] private RelativeStat _damage;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private UpgradableStatFloat _attackInterval;
    [SerializeField] private UpgradableStatInt _tickCount;
    [SerializeField] private UpgradableStatFloat _reloadDelay;
    [SerializeField] private UpgradableStatInt _chance;
    [SerializeField] private Vector3 _offset;

    private StormCloud _entity;

    public override void Upgrade()
    {
        _damage.Upgrade();
        _attackInterval.Upgrade();
        _tickCount.Upgrade();
        _reloadDelay.Upgrade();
        _chance.Upgrade();
    }

    public override void Apply()
    {
        if (Random.Range(1, 100) < _chance.Value == false)
            return;

        if (_entity == null)
        {
            _entity = SpellArgsProvider.Instance.GameFactory.CreateStormCloud(Target.transform.position + _offset);
        }
        else if (_entity.Reloaded == false)
        {
            return;
        }

        _entity.Init(Owner, _damage.Stat.Value, _damageType, _attackInterval.Value, _tickCount.Value, _reloadDelay.Value);
        _entity.StartAttacking();
    }

    public void DestroyEntity()
    {
        if (_entity == null)
            return;

        Object.Destroy(_entity);
    }
}
