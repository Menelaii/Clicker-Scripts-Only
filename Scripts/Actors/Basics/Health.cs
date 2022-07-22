using System;
using UnityEngine;

public class Health : IDamageable, IBodyChangesHandler
{
    public int Value { get; private set; }
    public int MaxValue { get; private set; }

    public event Action<int, int> ValueChanged;
    public event Action<int, DamageType> DamageTaken;
    public event Action<Health> Died;

    public Health(int maxValue, int value)
    {
        MaxValue = maxValue;
        Value = value;
    }

    public Health(int maxValue) : this(maxValue, maxValue) { }

    public void TakeDamage(int damage, DamageType type)
    {
        Value = Mathf.Clamp(Value - damage, 0, MaxValue);

        ValueChanged?.Invoke(Value, MaxValue);
        DamageTaken?.Invoke(damage, type);

        if (Value <= 0)
            Died?.Invoke(this);
    }

    public void Heal(int amount)
    {
        Value = Mathf.Clamp(Value + amount, 0, MaxValue);
        ValueChanged?.Invoke(Value, MaxValue);
    }

    public void OnBodyChanged(Body body)
    {
        MaxValue = body.Corpus.MaxHealth;
        Value = Mathf.Clamp(Value, 0, MaxValue);
        ValueChanged?.Invoke(Value, MaxValue);
    }
}
