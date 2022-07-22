using System.Collections;
using UnityEngine;

public class StormCloud : MonoBehaviour
{
    [SerializeField] private StormCloudAnimator _animator;

    private int _damage;
    private DamageType _damageType;
    private Combatant _owner;
    private float _attackInterval;
    private int _tickCount;
    private float _reloadDelay;
    private Coroutine _attackTask;

    public bool Reloaded { get; private set; } = true;

    public void Init(Combatant owner, int damage, DamageType damageType,
        float attackInterval, int tickCount, float reloadDelay)
    {
        _damage = damage;
        _damageType = damageType;
        _owner = owner;
        _attackInterval = attackInterval;
        _tickCount = tickCount;
        _reloadDelay = reloadDelay;
    }

    public void StartAttacking()
    {
        gameObject.SetActive(true);

        if (_attackTask != null)
            StopCoroutine(_attackTask);

        _attackTask = StartCoroutine(DamageWithInterval());
    }

    private void OnAttack()
    {
        _owner.Target.TakeDamage(_damage, _damageType);
    }

    private IEnumerator DamageWithInterval()
    {
        Reloaded = false;

        WaitForSeconds waitForInterval = new WaitForSeconds(_attackInterval);
        while (_tickCount > 0)
        {
            _animator.StartAttack();
            _tickCount--;
            yield return waitForInterval;
        }

        OnAttackTaskCompleted();
    }

    private void OnAttackTaskCompleted()
    {
        gameObject.SetActive(false);
        CoroutineMaster.Instance.InvokeAfterDelay(() => Reloaded = true, _reloadDelay);
    }
}
