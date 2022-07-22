using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BodyPartUIView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _label;

    public BodyPart BodyPart { get; private set; }

    public virtual void Init(BodyPart bodyPart)
    {
        BodyPart = bodyPart;
        _icon.sprite = bodyPart.Icon;
        _label.text = bodyPart.Label;
    }
}
