using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccumulatorEntity : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _damageButton;
    [SerializeField] private Button _healButton;

    private int _accumulated;
    private DamageType _damageType;
    private Combatant _owner;

    private void Awake()
    {
        _damageButton.onClick.AddListener(() => Damage());
        _healButton.onClick.AddListener(() => Heal());
    }

    private void OnDestroy()
    {
        _damageButton.onClick.RemoveListener(() => Damage());
        _healButton.onClick.RemoveListener(() => Heal());
    }

    public void Init(Combatant owner, DamageType damageType)
    {
        _owner = owner;
        _damageType = damageType;
    }

    public void Accumulate(int amount)
    {
        SetAccumulated(_accumulated + amount);
    }

    public void Damage()
    {
        if (_owner.Target.IsAlive == false)
            return;

        _owner.Target.TakeDamage(_accumulated, _damageType);
        SetAccumulated(0);
    }

    public void Heal()
    {
        if (_owner.IsAlive == false)
            return;

        _owner.Health.Heal(_accumulated);
        SetAccumulated(0);
    }

    private void SetAccumulated(int value)
    {
        _accumulated = value;
        _text.text = _accumulated.ToString();
    }
}
