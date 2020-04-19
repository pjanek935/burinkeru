using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class RevolverWeapon : WeaponBase
{
    RigWithGunAnimationController rigAnmationController;
    const int MAX_BULLETS = 6;
    
    public int Bullets
    {
        get;
        private set;
    }

    public override void Init(RigManager rigManager, BurinkeruCharacterController characterController, ParticlesManager particlesManager)
    {
        base.Init(rigManager, characterController, particlesManager);

        rigAnmationController = rigManager.RigWithRevolver;
        rigAnmationController.OnAttackEnded += onAttackEnded;
        rigAnmationController.OnCustomEvent += onCustomEvent;

        Bullets = MAX_BULLETS;
        CanAttack = true;
    }

    public override void Enter ()
    {
        base.Enter ();

        CanAttack = true;
    }

    void onAttackEnded()
    {
        CanAttack = true;

        switch (CurrentActionType)
        {
            case WeaponActionType.RELOAD:

                Bullets = MAX_BULLETS;

                break;
        }
    }

    void onCustomEvent (string parameter)
    {
        if (! string.IsNullOrEmpty (parameter))
        {
            if (string.Equals (parameter, "Reload"))
            {
                Vector3 forward = rigAnmationController.ClipOriginTransform.forward;
                Vector3 upward = rigAnmationController.ClipOriginTransform.up;
                Vector3 position = rigAnmationController.ClipOriginTransform.position;

                particleManager.ClipParticleManager.ShootParticle(position, upward, forward);
            }
            else if (string.Equals(parameter, "ShowClip"))
            {
                rigAnmationController.ClipGameObject.SetActive(true);
            }
            else if (string.Equals(parameter, "HideClip"))
            {
                rigAnmationController.ClipGameObject.SetActive(false);
            }
        }
    }

    void shoot()
    {
        Bullets--;

        Vector3 forward = Vector3.forward;
        Vector3 upward = Vector3.up;
        Vector3 position = Vector3.zero;

        if (characterController.IsSliding())
        {
            forward = rigAnmationController.GunTransformWhileSliding.forward;
            upward = rigAnmationController.GunTransformWhileSliding.up;
            position = rigAnmationController.GunTransformWhileSliding.position;
        }
        else
        {
            forward = rigAnmationController.GunTransform.forward;
            upward = rigAnmationController.GunTransform.up;
            position = rigAnmationController.GunTransform.position;
        }

        hangInAirIfNeeded ();

        particleManager.BulletsManager.Shoot(position, forward, upward);
        particleManager.SmokeParticleManager.ShootParticle(position + upward * 1.5f, upward, -forward);
        ShakeEffect.Instance.ShakeIfNotShaking(0.35f);
    }

    protected override void initActionsDefinitions()
    {
        CombatActionDefinition shoot = new CombatActionDefinition();
        shoot.Add(BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<int, CombatActionDefinition>((int) WeaponActionType.SHOOT, shoot));

        CombatActionDefinition reload = new CombatActionDefinition();
        reload.Add(BurinkeruInputManager.InputCommand.RELOAD);
        actionDefinitions.Add(new KeyValuePair<int, CombatActionDefinition>((int) WeaponActionType.RELOAD, reload));
    }

    protected override void requestAction(int actionIndex)
    {
        WeaponActionType actionType = (WeaponActionType) actionIndex;

        if (rigAnmationController != null && rigAnmationController == rigManager.CurrentRig)
        {
            if (CanAttack)
            {
                CanAttack = false;

                switch (actionType)
                {
                    case WeaponActionType.SHOOT:

                        if (Bullets > 0)
                        {
                            CurrentActionType = WeaponActionType.SHOOT;
                            rigAnmationController.Attack();
                            shoot();
                        }
                        else
                        {
                            CurrentActionType = WeaponActionType.RELOAD;
                            rigAnmationController.Reload();
                        }

                        break;

                    case WeaponActionType.RELOAD:

                        CurrentActionType = WeaponActionType.RELOAD;
                        rigAnmationController.Reload();

                        break;
                }
            }
        }
    }
}
