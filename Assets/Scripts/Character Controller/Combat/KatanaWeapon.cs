using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaWeapon : WeaponBase
{
    public enum KatanaAttackState
    {
        NONE = 0, SLASH_1, SLASH_2, SLASH_3, UPPERCUT, STAB, SIMPLE_ATTACK
    }

    RigWithKatanaAnimationController rigAnmationController;

    public KatanaAttackState CurrentState
    {
        get { return (KatanaAttackState)CurrentStateIndex; }
    }

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
        actionDefinitions.Add (new KeyValuePair<int, CombatActionDefinition> ((int) KatanaAttackState.STAB, stab));

        CombatActionDefinition uppercut = new CombatActionDefinition();
        uppercut.Add(BurinkeruInputManager.InputCommand.BACKWARD);
        uppercut.Add(BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<int, CombatActionDefinition> ((int)KatanaAttackState.UPPERCUT, uppercut));

        CombatActionDefinition uppercut2 = new CombatActionDefinition();
        uppercut2.Add(BurinkeruInputManager.InputCommand.BACKWARD, BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<int, CombatActionDefinition> ((int)KatanaAttackState.UPPERCUT, uppercut2));

        CombatActionDefinition uppercut3 = new CombatActionDefinition ();
        uppercut3.Add (BurinkeruInputManager.InputCommand.BACKWARD);
        uppercut3.Add (BurinkeruInputManager.InputCommand.JUMP);
        uppercut3.Add (BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add (new KeyValuePair<int, CombatActionDefinition> ((int) KatanaAttackState.UPPERCUT, uppercut3));

        CombatActionDefinition simpleAttact = new CombatActionDefinition();
        simpleAttact.Add(BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<int, CombatActionDefinition> ((int)KatanaAttackState.SIMPLE_ATTACK, simpleAttact));
    }

    void onAttackEnded ()
    {
        Debug.Log("onAttackEnded");
        CanAttack = true;
    }

    void onAttackStarted ()
    {
        Debug.Log("onAttackStarted: " + CurrentState);

        hangInAirIfNeeded ();

        switch (CurrentState)
        {
            case KatanaAttackState.UPPERCUT:

               // addVelocity(new Vector3 (0, 15f, 0));

                break;

            case KatanaAttackState.STAB:
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

            case KatanaAttackState.SIMPLE_ATTACK:

                {
                    Vector3 lookDirection = characterController.GetLookDirection();
                    lookDirection.Scale(BurinkeruCharacterController.MovementAxes);
                    //addVelocity(lookDirection * 5f);
                }

                break;
        }

        CurrentStateIndex = DEFAULT_STATE_INDEX;
    }

    protected override void requestAction (int actionIndex)
    {
        KatanaAttackState action = (KatanaAttackState)actionIndex;

        if (rigAnmationController != null && rigAnmationController == rigManager.CurrentRig)
        {
            RigWithKatanaAnimationController rigWithKatanaAnimationController = (RigWithKatanaAnimationController)rigManager.CurrentRig;

            if (CanAttack)
            {
                CanAttack = false;

                switch (action)
                {
                    case KatanaAttackState.SIMPLE_ATTACK:

                        CurrentStateIndex =  (int)KatanaAttackState.SIMPLE_ATTACK;
                        rigWithKatanaAnimationController.Attack();

                        break;

                    case KatanaAttackState.STAB:

                        CurrentStateIndex = (int)KatanaAttackState.STAB;
                        rigWithKatanaAnimationController.Stab();

                        break;

                    case KatanaAttackState.UPPERCUT:

                        CurrentStateIndex = (int)KatanaAttackState.UPPERCUT;
                        rigWithKatanaAnimationController.Uppercut();

                        break;
                }
            }
        }
    }

    void makeSimpleAttack ()
    {

    }

    void makeUpperCut ()
    {

    }

    void makePierce ()
    {

    }

    public void StartCut (int index)
    {

    }
}
