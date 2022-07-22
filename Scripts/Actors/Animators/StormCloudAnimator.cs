using UnityEngine;

public class StormCloudAnimator : MonoBehaviour
{
    private static readonly int _attack = Animator.StringToHash("Attack");

    [SerializeField] private Animator _animator;

    public void StartAttack()
    {
        _animator.SetTrigger(_attack);
    }
}
