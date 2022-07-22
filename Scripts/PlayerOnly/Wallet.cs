using System;
using UnityEngine;

[Serializable]
public class Wallet
{
    [SerializeField] private int _gold;

    public event Action<int> GoldChanged;

    public int Gold => _gold;

    public bool TryBuy(int price)
    {
        if (price > _gold)
            return false;

        _gold -= price;
        GoldChanged?.Invoke(_gold);

        return true;
    }

    public void Add(int amount)
    {
        _gold += amount;
        GoldChanged?.Invoke(_gold);
    }
}