using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CombatantAnimator))]
public class Combatant : MonoBehaviour, IDamageable, IBodyChangesHandler
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private DamageType _damageType;

    private int _damage;
    private int _armor;
    private int _critDamageInPercent;
    private int _critChance;
    private int _dodgeChance;
    private bool _isStunned;
    private bool _isAttacking;
    private int _modifiedDamage;
    private Health _health;
    private CombatantAnimator _animator;

    public IReadOnlyList<Spell> AttackModifiers { get; private set; }
    public Combatant Target { get; private set; }
    public AutoAttacker AutoAttacker { get; private set; }
    public SpellUser SpellUser { get; private set; }
    public Health Health => _health;
    public int Damage => _damage;
    public bool IsAlive => _health.Value > 0;
    public bool CanAttack => Target != null && Target.IsAlive && _isStunned == false;
    public int AutoAttackDamageCut => Mathf.CeilToInt(_damage * AutoAttacker.DamageCut);

    public event Action<Combatant> DieAnimationEnded;
    public event Action<Combatant> TargetChanged;

    private void Start()
    {
        AutoAttacker.Start(() => TryStartAttack(AutoAttackDamageCut));
    }

    private void OnDisable()
    {
        _health.ValueChanged -= _healthBar.OnValueChanged;
        _health.Died -= OnDied;

        AutoAttacker.Stop();
    }

    public void Init(SpellUser abilityuser, AutoAttacker autoAttacker, Health health, int damage, int armor,
        int critDamageInPercent, int critChance, int dodgeChance, IReadOnlyList<Spell> attackModifiers)
    {
        _damage = damage;
        _armor = armor;
        _critDamageInPercent = critDamageInPercent;
        _critChance = critChance;
        _dodgeChance = dodgeChance;
        _animator = GetComponent<CombatantAnimator>();

        AutoAttacker = autoAttacker;
        SpellUser = abilityuser;
        AttackModifiers = attackModifiers;

        InitHealth(health);
    }

    public void Init(Body body, Health health, AutoAttacker autoAttacker, SpellUser abilityuser)
    {
        InitHealth(health);
        SetStatsFrom(body);
        _animator = GetComponent<CombatantAnimator>();

        AutoAttacker = autoAttacker;
        SpellUser = abilityuser;
    }

    public void TakeDamage(int damage, DamageType type)
    {
        if (type.Avoidable && TryChance(_dodgeChance))
        {
            _animator.OnDodged();
            return;
        }

        if (type.Affectable)
            damage = AffectDamage(damage);

        _health.TakeDamage(damage, type);
        _animator.OnDamageTaken(damage, type);
    }

    public void SetTarget(Combatant target)
    {
        Target = target;
        TargetChanged?.Invoke(target);
    }

    public void TryStartAttack(int damageCut)
    {
        if (CanAttack == false || _isAttacking)
            return;

        _modifiedDamage = _damage - damageCut;

        StartAttack();
    }

    public void TryStartAttack()
    {
        if (CanAttack == false || _isAttacking)
            return;

        _modifiedDamage = _damage;

        StartAttack();
    }

    public void OnAttackAnimation()
    {
        if (CanAttack == false)
            return;

        Attack();
    }

    public void ModifyDamage(int amount)
    {
        _modifiedDamage += amount;
    }

    public void OnDiedAnimationEnded()
    {
        DieAnimationEnded?.Invoke(this);
    }

    public void Stun()
    {
        _animator.OnStunned();
        _isStunned = true;
    }

    public void UnStun()
    {
        _animator.OnUnstunned();
        _isStunned = false;
    }

    public void OnBodyChanged(Body body)
    {
        _health.OnBodyChanged(body);
        SpellUser.OnBodyChanged(body);
        AutoAttacker.OnBodyChanged(body);
        SetStatsFrom(body);
    }

    public void OnRespawn()
    {
        _health.Died += OnDied;
        
        SpellUser.ManaRegeneration.StartRegeneration();
        AutoAttacker.Start(() => TryStartAttack(AutoAttackDamageCut));
        _animator.OnRespawned();
    }

    private void StartAttack()
    {
        _isAttacking = true;
        _animator.OnAttack();
    }

    private void Attack()
    {
        _modifiedDamage += TryChance(_critChance) ? GetCritDamage(_damage) : 0;

        SpellUser.TryUse(AttackModifiers);

        Target.TakeDamage(_modifiedDamage, _damageType);

        _isAttacking = false;
    }

    private int AffectDamage(int damage) => damage - (damage * _armor / 100);

    private int GetCritDamage(int damage) => damage * _critDamageInPercent / 100;

    private bool TryChance(int chance) => UnityEngine.Random.Range(1, 100) < chance;

    private void OnDied(Health health)
    {
        health.Died -= OnDied;

        AutoAttacker.Stop();
        SpellUser.ManaRegeneration.StopRegeneration();
        _animator.OnDied();
    }

    private void SetStatsFrom(Body body)
    {
        _damage = body.Hands.Damage;
        _armor = body.Corpus.Armor;
        _critDamageInPercent = body.Legs.CritDamageInPercent;
        _critChance = body.Legs.CritChance;
        _dodgeChance = body.Legs.DodgeChance;
        AttackModifiers = body.Hands.Spells;
    }

    private void InitHealth(Health health)
    {
        _health = health;
        _health.ValueChanged += _healthBar.OnValueChanged;
        _healthBar.OnValueChanged(_health.MaxValue, _health.MaxValue);
        _health.Died += OnDied;
    }
}
