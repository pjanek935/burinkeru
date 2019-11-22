using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverWeapon : WeaponBase
{
    public enum RevolverAttackState
    {
        NONE = 0, SHOOT, RELOAD,
    }

    RigWithGunAnimationController rigAnmationController;
    const int MAX_BULLETS = 6;
    
    public int Bullets
    {
        get;
        private set;
    }

    public RevolverAttackState CurrentState
    {
        get { return (RevolverAttackState)CurrentStateIndex; }
    }

    public override void Init(RigManager rigManager, BurinkeruCharacterController characterController, ParticlesManager particlesManager)
    {
        base.Init(rigManager, characterController, particlesManager);

        rigAnmationController = rigManager.RigWithRevolver;
        rigAnmationController.OnAttackEnded += onAttackEnded;
        rigAnmationController.OnAttackStarted += onAttackStarted;

        Bullets = MAX_BULLETS;
    }

    void onAttackEnded()
    {
        Debug.Log("onAttackEnded");
        CanAttack = true;

        switch (CurrentState)
        {
            case RevolverAttackState.RELOAD:

                Debug.Log("Reload");
                Bullets = MAX_BULLETS;

                break;
        }
    }

    void onAttackStarted()
    {
        Debug.Log("onAttackStarted: " + CurrentState);

        switch (CurrentState)
        {
            case RevolverAttackState.SHOOT:

                shoot();

                break;
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

        particleManager.BulletsManager.ShootParticle(position, forward, upward);
    }

    protected override void initActionsDefinitions()
    {
        CombatActionDefinition shoot = new CombatActionDefinition();
        shoot.Add(BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<int, CombatActionDefinition>((int)RevolverAttackState.SHOOT, shoot));

        CombatActionDefinition reload = new CombatActionDefinition();
        reload.Add(BurinkeruInputManager.InputCommand.RELOAD);
        actionDefinitions.Add(new KeyValuePair<int, CombatActionDefinition>((int)RevolverAttackState.RELOAD, reload));
    }

    protected override void requestAction(int actionIndex)
    {
        RevolverAttackState action = (RevolverAttackState)actionIndex;

        if (rigAnmationController != null && rigAnmationController == rigManager.CurrentRig)
        {
            if (CanAttack)
            {
                CanAttack = false;

                switch (action)
                {
                    case RevolverAttackState.SHOOT:

                        if (Bullets > 0)
                        {
                            CurrentStateIndex = (int)RevolverAttackState.SHOOT;
                            rigAnmationController.Attack();
                        }
                        else
                        {
                            CurrentStateIndex = (int)RevolverAttackState.RELOAD;
                            rigAnmationController.Reload();
                        }

                        break;

                    case RevolverAttackState.RELOAD:

                        CurrentStateIndex = (int)RevolverAttackState.RELOAD;
                        rigAnmationController.Reload();

                        break;
                }
            }
        }
    }
}
