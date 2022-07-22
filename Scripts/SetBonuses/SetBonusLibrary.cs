using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Bonus Library", menuName = "Custom/Single/SetBonusLibrary")]
public class SetBonusLibrary : ScriptableObject
{
    [SerializeField] private List<SetBonus> _setBonuses;

    public static SetBonusLibrary Instance { get; private set; }

    public void SetInstance()
    {
        Instance = Instance == null 
            ? this 
            : throw new InvalidOperationException();
    }

    public SetBonus GetElement(SetId id)
    {
        return _setBonuses.Find(x => x.SetId == id);
    }
}
