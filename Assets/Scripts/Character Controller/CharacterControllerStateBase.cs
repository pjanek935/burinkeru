using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterControllerStateBase
{
    public delegate void CharacterControllerStateRequestNewState(CharacterControllerStateBase newState);
    public event CharacterControllerStateRequestNewState OnSetNewStateRequested;

    protected CharacterControllerStateBase previousState = null;
    protected BurinkeruInputManager inputManager;
    protected BurinkeruCharacterController characterController;
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
        this.characterController = parent;
        this.components = components;

        onEnter();
    }

    public void Exit ()
    {
        onExit();
    }

    protected virtual void setNewState (CharacterControllerStateBase newState)
    {
        characterController.SetNewState(newState);
    }

    protected void addVelocity (Vector3 velocityDelta)
    {
        characterController.AddVelocity(velocityDelta);
    }

    protected void setVelocity (Vector3 newVelocity)
    {
        characterController.SetVelocity(newVelocity);
    }

    protected void move (Vector3 deltaPosition)
    {
        characterController.Move(deltaPosition);
    }

    protected virtual void switchCrouch()
    {
        if (characterController.IsCrouching)
        {
            characterController.ExitCrouch();
        }
        else
        {
            characterController.EnterCrouch();
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
