using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaWeapon : WeaponBase
{
    public enum KatanaAttackState
    {
        NONE = 0, SLASH_1, SLASH_2, SLASH_3, UPPERCUT, STAB, SIMPLE_ATTACK
    }

    List<KeyValuePair<KatanaAttackState, CombatActionDefinition>> actionDefinitions = new List<KeyValuePair<KatanaAttackState, CombatActionDefinition>>();

    bool canAttack = true;

    public KatanaAttackState CurrentState
    {
        get;
        private set;
    }

    public KatanaWeapon (RigAnimationController rigAnimationController, BurinkeruCharacterController characterController)
        : base (rigAnimationController, characterController)
    {
        initActionDefinitions();
        rigAnimationController.OnAttackEnded += onAttackEnded;
        rigAnimationController.OnAttackStarted += onAttackStarted;
    }

    void initActionDefinitions ()
    {
        CombatActionDefinition stab = new CombatActionDefinition();
        stab.Add(BurinkeruInputManager.InputCommand.FORWARD);
        stab.Add(BurinkeruInputManager.InputCommand.FORWARD);
        stab.Add(BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<KatanaAttackState, CombatActionDefinition> (KatanaAttackState.STAB, stab));

        CombatActionDefinition stab2 = new CombatActionDefinition();
        stab2.Add(BurinkeruInputManager.InputCommand.FORWARD);
        stab2.Add(BurinkeruInputManager.InputCommand.FORWARD, BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<KatanaAttackState, CombatActionDefinition> (KatanaAttackState.STAB, stab2));

        CombatActionDefinition uppercut = new CombatActionDefinition();
        uppercut.Add(BurinkeruInputManager.InputCommand.FORWARD);
        uppercut.Add(BurinkeruInputManager.InputCommand.BACKWARD);
        uppercut.Add(BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<KatanaAttackState, CombatActionDefinition> (KatanaAttackState.UPPERCUT, uppercut));

        CombatActionDefinition uppercut2 = new CombatActionDefinition();
        uppercut2.Add(BurinkeruInputManager.InputCommand.FORWARD);
        uppercut2.Add(BurinkeruInputManager.InputCommand.BACKWARD, BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<KatanaAttackState, CombatActionDefinition> (KatanaAttackState.UPPERCUT, uppercut2));

        CombatActionDefinition simpleAttact = new CombatActionDefinition();
        simpleAttact.Add(BurinkeruInputManager.InputCommand.ATTACK);
        actionDefinitions.Add(new KeyValuePair<KatanaAttackState, CombatActionDefinition> (KatanaAttackState.SIMPLE_ATTACK, simpleAttact));
    }

    public override bool CheckForInput(InputBuffer inputBuffer)
    {
        bool result = false;
        KatanaAttackState detectedAction = getDetectedAction(inputBuffer);

        if (detectedAction != KatanaAttackState.NONE)
        {
            requestAction(detectedAction);
            result = true;
        }

        return result;
    }

    void onAttackEnded ()
    {
        Debug.Log("onAttackEnded");
        canAttack = true;
    }

    void onAttackStarted ()
    {
        Debug.Log("onAttackStarted: " + CurrentState);

        switch (CurrentState)
        {
            case KatanaAttackState.UPPERCUT:

                addVelocity(new Vector3 (0, 7f, 0));

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
                    addVelocity(lookDirection * 5f);
                }

                break;
        }
    }

    void requestAction (KatanaAttackState action)
    {
        if (canAttack)
        {
            canAttack = false;

            switch (action)
            {
                case KatanaAttackState.SIMPLE_ATTACK:

                    CurrentState = KatanaAttackState.SIMPLE_ATTACK;
                    rigAnimationController.Attack();

                    break;

                case KatanaAttackState.STAB:

                    CurrentState = KatanaAttackState.STAB;
                    rigAnimationController.Stab();

                    break;

                case KatanaAttackState.UPPERCUT:

                    CurrentState = KatanaAttackState.UPPERCUT;
                    rigAnimationController.Uppercut();

                    break;
            }
        }
    }

    KatanaAttackState getDetectedAction (InputBuffer inputBuffer)
    {
        KatanaAttackState detectedAction = KatanaAttackState.NONE;
        int lastDefinitionLength = 0;

        foreach (KeyValuePair <KatanaAttackState, CombatActionDefinition> entry in actionDefinitions)
        {
            if (inputBuffer.Matches(entry.Value))
            {
                if (entry.Value.Count > lastDefinitionLength)
                {
                    detectedAction = entry.Key;
                    lastDefinitionLength = entry.Value.Count;
                }
            }
        }

        return detectedAction;
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
