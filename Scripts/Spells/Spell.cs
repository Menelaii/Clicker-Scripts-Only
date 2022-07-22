using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Spell : ISerializationCallbackReceiver
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _label;
    [SerializeField] private UpgradableStatFloat _cooldown;
    [SerializeField] private UpgradableStatFloat _manaCostInPercent;

    [HideInInspector] [SerializeField] 
    private List<SerializedData> _spellElementsSerialized;
    [HideInInspector] [SerializeField]
    private int _id;
    [HideInInspector] [SerializeField] 
    private bool _needEnemyToUse;

    private List<SpellElement> _spellElements;

    public int Id => _id;
    public float Cooldown => _cooldown.Value;
    public float ManaCostInPercent => _manaCostInPercent.Value;
    public Sprite Icon => _icon;
    public string Label => _label;
    public bool NeedEnemyToUse => _needEnemyToUse;

    public Spell(int id)
    {
        _id = id;
    }

    public void SetElements(SpellElement[] spellElements)
    {
        _spellElements = spellElements == null 
            ? null
            : spellElements.ToList();

        CheckIfEnemyRequired(spellElements);
    }

    public void SetElements(SpellElementContainer[] containers)
    {
        SetElements(GetItemsFromContainers(containers));
    }

    public void Apply()
    {
        for (int i = 0; i < _spellElements.Count; i++)
        {
            _spellElements[i].Apply();
        }
    }

    public void Upgrade()
    {
        foreach (var effect in _spellElements)
        {
            effect.Upgrade();
        }

        _manaCostInPercent.Upgrade();
        _cooldown.Upgrade();
    }

    public void DestroyCreatedEntities()
    {
        foreach (SpellElement spellElement in _spellElements)
        {
            (spellElement as ICreateSpellEntity)?.DestroyEntity();
        }
    }

    public void OnBeforeSerialize()
    {
        _spellElementsSerialized = AbstractObjectsSerializer.Serialize(_spellElements);
    }

    public void OnAfterDeserialize()
    {
        _spellElements = AbstractObjectsSerializer.Deserialize<SpellElement>(_spellElementsSerialized);
    }

    public override bool Equals(object obj)
    {
        Spell otherSpell = obj as Spell;
        return otherSpell != null && otherSpell.Id == _id;
    }

    public override int GetHashCode()
    {
        return 1969571243 + _id.GetHashCode();
    }

    private SpellElement[] GetItemsFromContainers(SpellElementContainer[] containers)
    {
        if(containers == null)
            return null;

        var spellElements = new SpellElement[containers.Length];
        for (int i = 0; i < containers.Length; i++)
        {
            if (containers[i] == null)
                continue;

            spellElements[i] = containers[i].Item;
        }

        return spellElements;
    }

    private void CheckIfEnemyRequired(SpellElement[] spellElements)
    {
        if (spellElements == null)
            return;

        foreach (var spellElement in spellElements)
        {
            if (spellElement == null)
                continue;

            if (spellElement.TargetName == TargetType.Enemy)
            {
                _needEnemyToUse = true;
                break;
            }
        }
    }
}
