using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Custom/Spell")]
public class SpellContainer : Container<Spell>
{
    [SerializeField] private Spell _spell;
    [SerializeField] private SpellElementContainer[] _spellElements;

    public override Spell Item => _spell;

    private void Awake()
    {
        if (_spell != null && _spell.Id != 0)
            return;

        _spell = new Spell(GetInstanceID());
    }

    private void OnValidate()
    {
        _spell.SetElements(_spellElements);
    }
}
