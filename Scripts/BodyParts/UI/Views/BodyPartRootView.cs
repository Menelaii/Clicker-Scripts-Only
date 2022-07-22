using UnityEngine;

public class BodyPartRootView : BodyPartView
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _hands;
    [SerializeField] private Transform _legs;

    public Transform Head => _head;
    public Transform Hands => _hands;
    public Transform Legs => _legs;
}
