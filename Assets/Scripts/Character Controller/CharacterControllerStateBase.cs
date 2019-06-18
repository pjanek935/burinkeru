using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterControllerStateBase
{
    public delegate void CharacterControllerStateRequestNewState(CharacterControllerStateBase newState);
    public event CharacterControllerStateRequestNewState OnSetNewStateRequested;

    public delegate void CharacterControllerStateRequestVelocity(Vector3 value);
    public event CharacterControllerStateRequestVelocity OnAddVelocityRequested;
    public event CharacterControllerStateRequestVelocity OnSetVelocityRequested;

    protected CharacterControllerStateBase previousState = null;
    protected BurinkeruInputManager inputManager;
    protected BurinkeruCharacterController parent;
    protected CharacterComponents components;

    public bool IsTransitioning
    {
        get;
        protected set;
    }

    public void Enter (CharacterControllerStateBase previousState, BurinkeruInputManager inputManager, BurinkeruCharacterController parent, CharacterComponents components)
    {
        this.previousState = previousState;
        this.inputManager = inputManager;
        this.parent = parent;
        this.components = components;
        onEnter();

        Debug.Log("Enter state: " + this.GetType ().ToString ());
    }

    public void Exit ()
    {
        onExit();
    }

    protected virtual void setNewState (CharacterControllerStateBase newState)
    {
        OnSetNewStateRequested?.Invoke(newState);
    }

    protected void addVelocity (Vector3 velocityDelta)
    {
        OnAddVelocityRequested?.Invoke(velocityDelta);
    }

    protected void setVelocity (Vector3 newVelocity)
    {
        OnSetVelocityRequested?.Invoke(newVelocity);
    }

    protected abstract void onEnter();
    protected abstract void onExit();

    public abstract void UpdateMovement();
    public abstract void ApplyForces();
    public abstract float GetMovementSpeedFactor();
}
