using UnityEngine;

[CreateAssetMenu(fileName = "New Head", menuName = "Custom/BodyParts/Head")]
public class HeadContainer : BodyPartContainer
{
    [SerializeField] private Head _head;
    [SerializeField] private SpellContainer[] _spells;

    public override BodyPart Item
    {
        get
        {
            _head.Init(_spells);
            return _head;
        }
    }
}
