using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigWithKatanaAnimationController : RigAnimationController
{
    [SerializeField] ParticlesManager particlesManager;
    [SerializeField] ActivatableHitter hitter;

    public override void OnStartAttack(int index)
    {
        if (TimeManager.Instance.IsSlowMotionOn)
        {
            particlesManager.SlowMotionTrailsManager.RegisterNewSlash(index);
        }

        particlesManager.SlashTrailManager.ShootParticle(index);
        hitter.Activate();
        base.OnStartAttack(index);
    }

    public override void OnEndAttack()
    {
        base.OnEndAttack();
        hitter.Deactivate();
    }

    public void Uppercut()
    {
        animator.SetTrigger("Uppercut");
    }

    public void Stab()
    {
        animator.SetTrigger("Stab");
    }
}
