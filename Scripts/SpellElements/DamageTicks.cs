using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DamageTicks : SpellElement
{
    [SerializeField] private bool _stackable;
    [SerializeField] private RelativeStat _damage;
    [SerializeField] private UpgradableStatInt _damageLimit;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private UpgradableStatInt _ticksAmount;
    [SerializeField] private UpgradableStatFloat _tickInterval;

    private Coroutine _coroutine;
    private object _previousTarget;
    private int _tickDamage;
    private int _ticksLeft;
    private int _stackedTicksLeft;

    public override void Upgrade()
    {
        _damage.Upgrade();
        _damageLimit.Upgrade();
        _ticksAmount.Upgrade();
        _tickInterval.Upgrade();
    }

    public override void Apply()
    {
        if ((_coroutine != null && _stackable == false) || (_previousTarget != null && _previousTarget.Equals(Target) == false))
        {
            CoroutineMaster.Instance.StopCoroutine(_coroutine);
        }
        else if(_coroutine != null && _stackable && _ticksLeft > 0)
        {
            _tickDamage = Mathf.Clamp(_tickDamage + _damage.GetValue(Target, Owner), _damage.Stat.Value, _damageLimit.Value);

            _stackedTicksLeft = _ticksLeft > 0 ? _ticksLeft : 0;
            _ticksLeft += _ticksAmount.Value;

            return;
        }

        _previousTarget = Target;
        _ticksLeft = _ticksAmount.Value;

        _tickDamage = _damage.GetValue(Target, Owner);

        _stackedTicksLeft = 0;
        _coroutine = CoroutineMaster.Instance.StartCoroutine(DealDamageWithInterval(Target));
    }

    private IEnumerator DealDamageWithInterval(Combatant target)
    {
        WaitForSeconds waitForInterval = new WaitForSeconds(_tickInterval.Value);

        while (_ticksLeft > 0)
        {
            if (_stackedTicksLeft <= 0)
                _tickDamage = _damage.GetValue(Target, Owner);

            target.TakeDamage(_tickDamage, _damageType);

            _ticksLeft--;
            _stackedTicksLeft--;

            yield return waitForInterval;
        }
    }
}
