using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Spell Changes", menuName = "Custom/Bonuses/SpellChanges")]
public class SpellChanges : Bonus
{
    [SerializeField] private SpellContainer _spellContainer;
    [SerializeField] private SpellPlacementType _placement;
    [SerializeField] private ActionType _action;

    private Spell _spell;

    private static Dictionary<ActionType, Action<ISpellCollection, Spell>> _actions =
    new Dictionary<ActionType, Action<ISpellCollection, Spell>>
    {
        [ActionType.Add] = AddTo,
        [ActionType.Remove] = RemoveFrom
    };

    private static Dictionary<ActionType, Action<ISpellCollection, Spell>> _rollbackActions =
    new Dictionary<ActionType, Action<ISpellCollection, Spell>>
    {
        [ActionType.Add] = RollbackAdding,
        [ActionType.Remove] = RollbackRemoving
    };

    public BodyPartType PlacementBPType => ToBodyPartType(_placement);

    public void OnEnable()
    {
        if(_spellContainer != null)
        {
            _spell = _spellContainer.GetClone();
        }
    }

    public override void DisableOn(Body body)
    {
        _spell.DestroyCreatedEntities();

        ISpellCollection spellCollection = GetSpellPlacementOnDisable(body);
        RollBack(spellCollection);
    }

    public override void EnableOn(Body body)
    {
        ISpellCollection spellCollection = GetSpellPlacementOnEnable(body);
        ChangeSpellsIn(spellCollection);
    }

    private ISpellCollection GetSpellPlacementOnDisable(Body body)
    {
        return _placement switch
        {
            SpellPlacementType.Head => IsTargetBodyPartWasReplacedIn(body, PlacementBPType)
            ? (Head)body.LastReplacedPart
            : body.Head,

            SpellPlacementType.Hands => IsTargetBodyPartWasReplacedIn(body, PlacementBPType)
                ? (Hands)body.LastReplacedPart
                : body.Hands,

            _ => throw new InvalidOperationException()
        };
    }

    private ISpellCollection GetSpellPlacementOnEnable(Body body)
    {
        return _placement switch
        {
            SpellPlacementType.Head => body.Head,

            SpellPlacementType.Hands => body.Hands,

            _ => throw new InvalidOperationException()
        };
    }

    private void ChangeSpellsIn(ISpellCollection spellCollection)
    {
        _actions[_action].Invoke(spellCollection, _spell);
    }

    private void RollBack(ISpellCollection spellCollection)
    {
        _rollbackActions[_action].Invoke(spellCollection, _spell);
    }

    private static void RemoveFrom(ISpellCollection spellCollection, Spell spell)
    {
        spellCollection.Remove(spell);
    }

    private static void AddTo(ISpellCollection spellCollection, Spell spell)
    {
        spellCollection.Add(spell);
    }

    private static void RollbackAdding(ISpellCollection spellCollection, Spell spell)
    {
        spellCollection.Remove(spell);
    }

    private static void RollbackRemoving(ISpellCollection spellCollection, Spell spell)
    {
        spellCollection.AddRemoved(spell);
    }

    private BodyPartType ToBodyPartType(SpellPlacementType placement)
    {
        return (BodyPartType)((int)placement);
    }

    private enum ActionType
    {
        Add,
        Remove
    }

    private enum SpellPlacementType
    {
        Head = 0,
        Hands = 2
    }
}
