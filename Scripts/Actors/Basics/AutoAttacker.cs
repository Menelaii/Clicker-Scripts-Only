using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AutoAttacker : IBodyChangesHandler
{
    private static readonly int DEFAULT_DAMAGE_CUT = 0;

    private Action _onAttack;
    private float _attackCooldownSpread;
    private float _attackCooldown;
    private float _damageCut;
    private Coroutine _attackTask;

    public AutoAttacker(float attackCooldown, float attackCooldownSpread, float damageCut)
    {
        _attackCooldown = attackCooldown;
        _attackCooldownSpread = attackCooldownSpread;
        _damageCut = damageCut;
    }

    public AutoAttacker(float attackCooldown, float attackCooldownSpread)
        : this(attackCooldown, attackCooldownSpread, DEFAULT_DAMAGE_CUT) { }

    public float DamageCut => _damageCut;

    public void Start(Action onAttack)
    {
        if(_attackTask != null)
            CoroutineMaster.Instance.StopCoroutine(_attackTask);
            
        _onAttack = onAttack;
        _attackTask = CoroutineMaster.Instance.StartCoroutine(AttackWithCooldown());
    }

    public void Stop()
    {
        if (CoroutineMaster.Instance == null)
            return;

        CoroutineMaster.Instance.StopCoroutine(_attackTask);
    }

    public void IncreaseAttackCooldown(int percent)
    {
        _attackCooldown += _attackCooldown * percent / 100f;
    }

    public void OnBodyChanged(Body body)
    {
        _attackCooldown = body.Hands.AutoAttackCooldown;
    }

    private IEnumerator AttackWithCooldown()
    {
        var waitForCooldown = new WaitForSeconds(_attackCooldown + Random.Range(-_attackCooldownSpread, _attackCooldownSpread));
        while (true)
        {
            yield return waitForCooldown;
            _onAttack?.Invoke();
        }
    }
}
