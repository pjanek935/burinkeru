using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RigController : MonoBehaviour
{
    public delegate void CustomEventHandler(string parameter);
    public event CustomEventHandler OnCustomEvent;

    public delegate void OnAttackStartedEventHandler (int attackIndex);
    public OnAttackStartedEventHandler OnAttackStarted;

    public UnityAction OnAttackEnded;
    public UnityAction OnHideEnded;


    [SerializeField] protected Animator animator = null;

    public void SetWalk (bool walk)
    {
        animator.SetBool("Walk", walk);
    }

    public void SetInAir (bool inAir)
    {
        animator.SetBool("InAir", inAir);
    }

    public void SetSlide(bool slide)
    {
        animator.SetBool("Slide", slide);
    }

    public void SetWalkSpeed (float walkSpeed)
    {
        animator.SetFloat("WalkSpeed", walkSpeed);
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
        OnAttackStarted?.Invoke(index);
    }

    public virtual void OnEndAttack()
    {
        OnAttackEnded?.Invoke();
    }

    public virtual void OnEndHide ()
    {
        OnHideEnded?.Invoke();
    }

    public virtual void OnOtherEvent (string parameter)
    {
        OnCustomEvent?.Invoke(parameter);
    }

    public void SetTimeFactor (float timeFactor)
    {
        animator.SetFloat("TimeFactor", timeFactor);
    }

    public void SetTrigger (string trigger)
    {
        if (animator != null)
        {
            animator.SetTrigger (trigger);
        }
    }
}
