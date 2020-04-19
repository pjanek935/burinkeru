﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class KatanaWeapon : WeaponBase
{
    RigWithKatanaController rigAnmationController;

    public override void Init(RigManager rigManager, BurinkeruCharacterController characterController, ParticlesManager particlesManager)
    {
        base.Init(rigManager, characterController, particlesManager);

        rigAnmationController = rigManager.RigWithKatana;
        rigAnmationController.OnAttackEnded += onAttackEnded;
        rigAnmationController.OnAttackStarted += onAttackStarted;
    }

    protected override void initActionsDefinitions ()
    {
        CombatActionDefinition stab = new CombatActionDefinition ();
        stab.Add (BurinkeruInputManager.InputCommand.RUN);
        stab.Add (BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add (new KeyValuePair<int, CombatActionDefinition> ((int) WeaponActionType.STAB, stab));

        CombatActionDefinition uppercut = new CombatActionDefinition();
        uppercut.Add(BurinkeruInputManager.InputCommand.BACKWARD);
        uppercut.Add(BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<int, CombatActionDefinition> ((int) WeaponActionType.UPPERCUT, uppercut));

        CombatActionDefinition uppercut2 = new CombatActionDefinition();
        uppercut2.Add(BurinkeruInputManager.InputCommand.BACKWARD, BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<int, CombatActionDefinition> ((int) WeaponActionType.UPPERCUT, uppercut2));

        CombatActionDefinition uppercut3 = new CombatActionDefinition ();
        uppercut3.Add (BurinkeruInputManager.InputCommand.BACKWARD);
        uppercut3.Add (BurinkeruInputManager.InputCommand.JUMP);
        uppercut3.Add (BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add (new KeyValuePair<int, CombatActionDefinition> ((int) WeaponActionType.UPPERCUT, uppercut3));

        CombatActionDefinition simpleAttact = new CombatActionDefinition();
        simpleAttact.Add(BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<int, CombatActionDefinition> ((int) WeaponActionType.SLASH, simpleAttact));
    }

    void onAttackEnded ()
    {
        CanAttack = true;
    }

    void onAttackStarted (int attackIndex)
    {
        hangInAirIfNeeded ();

        switch (CurrentActionType)
        {
            case WeaponActionType.UPPERCUT:
                break;

            case WeaponActionType.STAB:
                {
                    Vector3 lookDirection = characterController.GetLookDirection();
                    lookDirection.Normalize();

                    if (characterController.IsGrounded)
                    {
                        lookDirection.Scale(BurinkeruCharacterController.MovementAxes);
                    }

                    addVelocity(lookDirection * 20);
                }

                break;

            case WeaponActionType.SLASH:
                break;
        }

        CurrentActionType = WeaponActionType.NONE;
    }

    protected override void requestAction (int actionIndex)
    {
        WeaponActionType action = (WeaponActionType) actionIndex;

        if (rigAnmationController != null && rigAnmationController == rigManager.CurrentRig)
        {
            RigWithKatanaController rigWithKatanaAnimationController = (RigWithKatanaController)rigManager.CurrentRig;

            if (CanAttack)
            {
                CanAttack = false;

                switch (action)
                {
                    case WeaponActionType.SLASH:

                        CurrentActionType = WeaponActionType.SLASH;
                        rigWithKatanaAnimationController.Attack();

                        break;

                    case WeaponActionType.STAB:

                        CurrentActionType = WeaponActionType.STAB;
                        rigWithKatanaAnimationController.Stab();

                        break;

                    case WeaponActionType.UPPERCUT:

                        CurrentActionType = WeaponActionType.UPPERCUT;
                        rigWithKatanaAnimationController.Uppercut();

                        break;
                }
            }
        }
    }
}
