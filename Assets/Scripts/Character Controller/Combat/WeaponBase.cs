using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public abstract class WeaponBase
{
    public delegate void VelocityEventHandler(Vector3 velocity);
    public event VelocityEventHandler OnAddVelocityRequested;
    public event VelocityEventHandler OnSetVelocityRequested;

    protected RigManager rigManager;
    protected BurinkeruCharacterController characterController;
    protected ParticlesManager particleManager;

    protected List<KeyValuePair<int, CombatActionDefinition>> actionDefinitions = new List<KeyValuePair<int, CombatActionDefinition>>();
    protected const int DEFAULT_STATE_INDEX = 0;

    public WeaponActionType CurrentActionType
    {
        get;
        protected set;
    }

    public bool CanAttack
    {
        get;
        protected set;
    }

    public virtual void Enter ()
    {

    }

    public virtual void Init (RigManager rigManager, BurinkeruCharacterController characterController, ParticlesManager particleManager)
    {
        this.rigManager = rigManager;
        this.characterController = characterController;
        this.particleManager = particleManager;
        CanAttack = true;

        initActionsDefinitions();
    }

    public bool CheckForInput (InputBuffer inputBuffer)
    {
        bool result = false;
        int detectedActionIndex = getDetectedActionIndex(inputBuffer);

        if (detectedActionIndex != DEFAULT_STATE_INDEX)
        {
            requestAction(detectedActionIndex);
            result = true;
        }

        return result;
    }

    protected int getDetectedActionIndex(InputBuffer inputBuffer)
    {
        int detectedActionIndex = DEFAULT_STATE_INDEX;
        int lastDefinitionLength = 0;

        foreach (KeyValuePair<int, CombatActionDefinition> entry in actionDefinitions)
        {
            if (inputBuffer.Matches(entry.Value))
            {
                if (entry.Value.Count > lastDefinitionLength)
                {
                    detectedActionIndex = entry.Key;
                    lastDefinitionLength = entry.Value.Count;
                }
            }
        }

        return detectedActionIndex;
    }

    protected void addVelocity (Vector3 dVelocity)
    {
        OnAddVelocityRequested?.Invoke(dVelocity);
    }

    protected void setVelocity (Vector3 velocity)
    {
        OnSetVelocityRequested?.Invoke(velocity);
    }

    protected void hangInAirIfNeeded ()
    {
        if (!characterController.IsGrounded)
        {
            Vector3 velocity = characterController.Velocity;

            if (velocity.y < 0)
            {
                velocity.y = 4.5f;
            }

            characterController.SetVelocity (velocity);
        }
    }

    protected abstract void initActionsDefinitions();
    protected abstract void requestAction(int actionIndex);
}
