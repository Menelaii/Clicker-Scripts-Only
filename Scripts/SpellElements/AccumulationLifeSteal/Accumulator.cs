using UnityEngine;

[System.Serializable]
public class Accumulator : SpellElement, ICreateSpellEntity
{
    [SerializeField] private RelativeStat _toAccumulate;
    [SerializeField] private DamageType _accumluatorDamageType;

    private AccumulatorEntity _entity;

    public override void Apply()
    {
        if (_entity == null)
            CreateAccumulator();

        if (_entity.gameObject.activeSelf == false)
            _entity.gameObject.SetActive(true);

        _entity.Accumulate(_toAccumulate.GetValue(Target, Owner));
    }

    public override void Upgrade()
    {
        _toAccumulate.Upgrade();
    }

    public void DestroyEntity()
    {
        if (_entity == null)
            return;

        Object.Destroy(_entity.gameObject);
    }

    private void CreateAccumulator()
    {
        _entity = SpellArgsProvider.Instance.GameFactory.CreateAccumulatorEntity();
        _entity.Init(Owner, _accumluatorDamageType);
    }
}
