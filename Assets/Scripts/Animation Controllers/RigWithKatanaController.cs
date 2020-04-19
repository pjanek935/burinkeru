using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class RigWithKatanaController : RigController
{
    [SerializeField] ActivatableHitter hitter;

    public override void OnStartAttack(int index)
    {
        ParticlesManager particlesManager = ParticlesManager.Instance;

        if (TimeManager.Instance.IsSlowMotionOn)
        {
            particlesManager.SlowMotionTrailsManager.RegisterNewSlash(index);
        }

        particlesManager.SlashTrailManager.ShootParticle(index);
        Hashtable parameters = new Hashtable ();
        WeaponActionType attackType = WeaponActionType.NONE;

        if (index == 4)
        {
            attackType = WeaponActionType.STAB;
        }
        else if (index == 3)
        {
            attackType = WeaponActionType.UPPERCUT;
        }
        else
        {
            attackType = WeaponActionType.SLASH;
        }

        parameters.Add ("AttackType", attackType);
        hitter.Activate(parameters);

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
