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
    public event CharacterControllerStateRequestVelocity OnMoveRequested;

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

        //Debug.Log("Enter state: " + this.GetType ().ToString ());
    }

    public void Exit ()
    {
        //Debug.Log("Exit state: " + this.GetType().ToString());
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

    protected void move (Vector3 deltaPosition)
    {
        if (OnMoveRequested != null)
        {
            OnMoveRequested(deltaPosition);
        }
    }

    protected virtual void switchCrouch()
    {
        if (parent.IsCrouching)
        {
            parent.ExitCrouch();
        }
        else
        {
            parent.EnterCrouch();
        }
    }

    protected void setReferences (CharacterControllerStateBase state)
    {
        if (state != null)
        {
            state.OnAddVelocityRequested += addVelocity;
            state.OnSetVelocityRequested += setVelocity;
            state.OnMoveRequested += move;
        }
    }

    protected void removeReferences(CharacterControllerStateBase state)
    {
        if (state != null)
        {
            state.OnAddVelocityRequested -= addVelocity;
            state.OnSetVelocityRequested -= setVelocity;
            state.OnMoveRequested -= move;
        }
    }

    protected abstract void onEnter();
    protected abstract void onExit();

    public virtual void OnSwitchToCrouch() { }
    public virtual float GetMovementDrag() { return 1f; }
    public abstract void UpdateMovement();
    public abstract void ApplyForces();
    public abstract float GetMovementSpeedFactor();
}
