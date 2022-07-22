using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class CombatantAnimator : MonoBehaviour
{
    private static readonly int AttackKey = Animator.StringToHash("Attack");
    private static readonly int DiedKey = Animator.StringToHash("Died");
    private static readonly int RespawnedKey = Animator.StringToHash("Respawned");
    //private static readonly int DamageTakenKey = Animator.StringToHash("DamageTaken");
    //private static readonly int StunnedKey = Animator.StringToHash("Stunned");
    //private static readonly int UnstunnedKey = Animator.StringToHash("UnStunned");

    [SerializeField] private Transform _center;

    private Animator _animator;
    private AttackResultViewer _attackResultViewer;

    public void Init(AttackResultViewer attackResultViewer)
    {
        _attackResultViewer = attackResultViewer;
        _animator = GetComponent<Animator>();
    }

    public virtual void OnDamageTaken(int damage, DamageType damageType)
    {
        if(_center == null)
            return;

        _attackResultViewer.DropDamageView(damage, damageType, _center.position);
    }

    public virtual void OnAttack()
    {
        _animator.SetTrigger(AttackKey);
    }

    public virtual void OnDied()
    {
        _animator.SetTrigger(DiedKey);
    }

    public virtual void OnRespawned()
    {
        _animator.SetTrigger(RespawnedKey);
    }

    public virtual void OnStunned()
    {
        //_animator.SetTrigger(StunnedKey);
    }

    public virtual void OnUnstunned()
    {
        //_animator.SetTrigger(UnstunnedKey);
    }

    public virtual void OnDodged()
    {
        if(_center == null)
            return;
            
        _attackResultViewer.DropDodgeView(_center.position);
    }
}
