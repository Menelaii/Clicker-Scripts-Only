using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Bar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _valueAndMaxValueText;
    [SerializeField] private TMP_Text _regenerationText;

    public void OnValueChanged(int value, int maxValue)
    {
        _slider.value = (float)value / maxValue;
        _valueAndMaxValueText.text = $"{value} / {maxValue}";
    }

    public void OnRegenerationChanged(int regeneration)
    {
        _regenerationText.text = "+" + regeneration;
    }
}
