using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStateBase
{
    protected CharacterControllerBase characterController;

    public void Enter (CharacterControllerBase parent)
    {
        this.characterController = parent;
        onEnter ();
    }

    public void Exit ()
    {
        onExit ();
    }

    protected virtual void setNewState (CharacterStateBase newState)
    {
        characterController.SetNewState (newState);
    }

    protected void addVelocity (Vector3 velocityDelta)
    {
        characterController.AddVelocity (velocityDelta);
    }

    protected void setVelocity (Vector3 newVelocity)
    {
        characterController.SetVelocity (newVelocity);
    }

    protected void move (Vector3 deltaPosition)
    {
        characterController.Move (deltaPosition);
    }

    protected abstract void onEnter ();
    protected abstract void onExit ();
    public virtual float GetMovementDrag () { return 1f; }
    public abstract void UpdateMovement ();
    public abstract void ApplyForces ();
    public abstract float GetMovementSpeedFactor ();
}
