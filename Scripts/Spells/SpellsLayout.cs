using System.Collections.Generic;
using UnityEngine;

public class SpellsLayout : MonoBehaviour, IBodyChangesHandler
{
    [SerializeField] private SpellView _template;
    [SerializeField] private GameObject _container;

    private Combatant _combatant;
    private IReadOnlyList<Spell> _spells;

    private void Start()
    {
        CreateViews();
    }

    public void Init(IReadOnlyList<Spell> spells, Combatant combatant)
    {
        _spells = spells;
        _combatant = combatant;
    }

    public void OnBodyChanged(Body body)
    {
        DestroyViews();
        CreateViews();
        _spells = body.Head.Spells;
    }

    private void CreateViews()
    {
        for (int i = 0; i < _spells.Count; i++)
        {
            var view = Instantiate(_template, _container.transform);

            view.Init(_spells[i], _combatant.SpellUser.Mana);
            view.Clicked += OnAbilityViewClicked;
            _combatant.SpellUser.Mana.ValueChanged += view.OnManaChanged;
        }
    }

    private void DestroyViews()
    {
        for (int i = 0; i < _container.transform.childCount; i++)
        {
            var view = _container.transform.GetChild(i).gameObject.GetComponent<SpellView>();
            _combatant.SpellUser.Mana.ValueChanged -= view.OnManaChanged;
            Destroy(view.gameObject);
        }
    }

    private void OnAbilityViewClicked(SpellView spellView)
    {
        if(_combatant.IsAlive == false)
        {
            return;
        }

        if(_combatant.SpellUser.TryUse(spellView.Ability))
        {
            spellView.SetCooldown();
        }
    }
}
