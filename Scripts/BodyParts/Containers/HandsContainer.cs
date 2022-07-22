using UnityEngine;

[CreateAssetMenu(fileName = "New Hands", menuName = "Custom/BodyParts/Hands")]
public class HandsContainer : BodyPartContainer
{
    [SerializeField] private Hands _hands;
    [SerializeField] private SpellContainer[] _attackModifiers;

    public override BodyPart Item
    {
        get
        {
            _hands.Init(_attackModifiers);
            return _hands;
        }
    }
}
