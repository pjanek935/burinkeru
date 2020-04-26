using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class RigWithKatanaController : RigController
{
    [SerializeField] ActivatableHitter hitter;

    private void OnEnable ()
    {
        if (hitter != null)
        {
            hitter.OnMessageRecived += onMessageFromHitterRecived;
        }
    }

    private void OnDisable ()
    {
        if (hitter != null)
        {
            hitter.OnMessageRecived -= onMessageFromHitterRecived;
        }
    }

    void onMessageFromHitterRecived (Hashtable parameters)
    {
        if (parameters != null)
        {
            if (parameters.ContainsKey (ParameterType.SENDER_TYPE))
            {
                SenderType senderType = (SenderType) parameters [ParameterType.SENDER_TYPE];

                switch (senderType)
                {
                    case SenderType.OBJECT:
                    case SenderType.NPC:

                        ShakeEffect.Instance.ShakeAndClampToGivenValue (0.5f);
                        ParticlesManager.Instance.SwordOnHitParticleManager.ShootParticle ();

                        break;
                }
            }

            if (parameters.ContainsKey (ParameterType.ATTACK_TYPE))
            {
                WeaponActionType actionType = (WeaponActionType) parameters [ParameterType.ATTACK_TYPE];

                switch (actionType)
                {
                    case WeaponActionType.BLOCK:

                        animator.SetTrigger ("BLOCKED");
                        hitter.Deactivate ();

                        break;
                }
            }
        }
    }

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
        else if (index == 5)
        {
            attackType = WeaponActionType.DOWNWARD_SMASH;
        }
        else
        {
            attackType = WeaponActionType.SLASH;
        }

        parameters.Add (ParameterType.ATTACK_TYPE, attackType);
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

    public void DownwardSmash ()
    {
        animator.SetTrigger ("DownwardSmash");
    }

    public void Stab()
    {
        animator.SetTrigger("Stab");
    }
}
