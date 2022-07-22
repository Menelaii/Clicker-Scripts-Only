using System;
using UnityEngine;

public class Mana : IBodyChangesHandler
{
    public int Value { get; private set; }
    public int MaxValue { get; private set; }

    public event Action<int, int> ValueChanged;

    public Mana(int value, int maxValue)
    {
        Value = value;
        MaxValue = maxValue;
    }

    public Mana(int maxValue) : this(maxValue, maxValue) { }

    public void Add(int amount)
    {
        Value = Mathf.Clamp(Value + amount, 0, MaxValue);
        ValueChanged?.Invoke(Value, MaxValue);
    }

    public void Use(float percent)
    {
        Value = (int)Mathf.Clamp(Value - (MaxValue * percent / 100f), 0, MaxValue);
        ValueChanged?.Invoke(Value, MaxValue);
    }

    public void Use(int amount)
    {
        Value = Mathf.Clamp(Value - amount, 0, MaxValue);
        ValueChanged?.Invoke(Value, MaxValue);
    }

    public bool IsEnoughFor(Spell ability)
    {
        return IsEnoughFor(ability.ManaCostInPercent, Value, MaxValue);
    }

    public bool IsEnoughFor(float manaCostInPercent)
    {
        return IsEnoughFor(manaCostInPercent, Value, MaxValue);
    }

    public static bool IsEnoughFor(float manaCostInPercent, int mana, int maxMana)
    {
        return mana - (maxMana * manaCostInPercent / 100f) >= 0;
    }

    public void OnBodyChanged(Body body)
    {
        MaxValue = body.Head.MaxMana;
        Value = Mathf.Clamp(Value, 0, MaxValue);
        ValueChanged?.Invoke(Value, MaxValue);
    }
}
