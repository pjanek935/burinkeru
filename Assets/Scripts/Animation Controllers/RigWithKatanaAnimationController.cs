using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigWithKatanaAnimationController : RigAnimationController
{
    [SerializeField] ParticlesManager particlesManager;
    [SerializeField] List<Transform> originTransforms = new List<Transform>();
    [SerializeField] List<ActivatableHitter> hitters = new List<ActivatableHitter>();
    [SerializeField] GameObject pierceParticle = null;

    public override void OnStartAttack(int index)
    {
        if (index >= 0 && index < originTransforms.Count && originTransforms [index] != null)
        {
            Vector3 postion = originTransforms[index].position;
            Vector3 forward = originTransforms[index].forward;
            Vector3 up = originTransforms[index].up;

            if (TimeManager.Instance.IsSlowMotionOn)
            {
                particlesManager.SlowMotionTrailsManager.ShootParticle(postion, forward, up);
                particlesManager.SlashTrailManager.RegisterNewSlash(postion, forward, up);
            }
            else
            {
                particlesManager.SlashTrailManager.ShootParticle(postion, forward, up);
            }
        }
        
        if (index >= 0 && index < hitters.Count && hitters [index] != null)
        {
            hitters[index].Activate();
        }

        base.OnStartAttack(index);
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
