using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigAnimationController : MonoBehaviour
{
    public UnityAction OnAttackEnded;
    public UnityAction OnAttackStarted;
    public UnityAction OnHideEnded;

    [SerializeField] protected Animator animator = null;

    public void SetWalk (bool walk)
    {
        animator.SetBool("Walk", walk);
    }
    
    public void Attack ()
    {
        animator.SetTrigger("Attack");
    }

    public void Hide ()
    {
        animator.SetTrigger("Hide");
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public virtual void OnStartAttack(int index)
    {
        OnAttackStarted?.Invoke();
    }

    public virtual void OnEndAttack()
    {
        OnAttackEnded?.Invoke();
    }

    public virtual void OnEndHide ()
    {
        OnHideEnded?.Invoke();
    }
}
