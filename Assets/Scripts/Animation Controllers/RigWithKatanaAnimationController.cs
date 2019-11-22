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

        Quaternion rotation = originTransforms[index].rotation;
        Vector3 postion = originTransforms[index].position;

        if (TimeManager.Instance.IsSlowMotionOn)
        {
            particlesManager.SlowMotionTrailsManager.ShootParticle(postion, originTransforms[index].forward, originTransforms[index].up);
            particlesManager.SlashTrailManager.RegisterNewSlash (postion, originTransforms[index].forward, originTransforms[index].up);
        }
        else
        {
            particlesManager.SlashTrailManager.ShootParticle(postion, originTransforms[index].forward, originTransforms[index].up);
        }
        
        hitters[index].Activate();

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
