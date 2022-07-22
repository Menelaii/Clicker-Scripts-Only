using UnityEngine;

[CreateAssetMenu(fileName = "New DamageType", menuName = "Custom/DamageType")]
public class DamageType : ScriptableObject
{
    [SerializeField] private Color _color;
    [SerializeField] private bool _avoidable;
    [SerializeField] private bool _affectable;

    public Color Color => _color;
    public bool Avoidable => _avoidable;
    public bool Affectable => _affectable;
}
