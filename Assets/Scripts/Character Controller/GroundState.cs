using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : CharacterControllerStateBase
{
    CharacterControllerStateBase groundedInternalState;

    public override void ApplyForces()
    {
        if (! parent.IsGrounded)
        {
            //bool canExitGroundState = true;

            //if (groundedInternalState != null && groundedInternalState is CrouchState && groundedInternalState.IsTransitioning)
            //{
            //    canExitGroundState = false;
            //}

            //if (canExitGroundState)
            //{
            //    setNewState(new InAirState());
            //}

            if (groundedInternalState != null && groundedInternalState is CrouchState)
            {
                ((CrouchState)groundedInternalState).ExitImmediate();
            }

            setNewState(new InAirState());
        }
    }

    protected override void setNewState(CharacterControllerStateBase newState)
    {
        if (groundedInternalState != null)
        {
            groundedInternalState.Exit();
        }

        base.setNewState(newState);
    }

    public override void UpdateMovement()
    {
        if (inputManager.IsCommandDown (BurinkeruInputManager.InputCommand.JUMP))
        {
            if (groundedInternalState != null && groundedInternalState is CrouchState)
            {
                groundedInternalState.Exit();
            }

            jump();
        }
        else if (inputManager.IsCommandDown (BurinkeruInputManager.InputCommand.CROUCH))
        {
            switchCrouch();
        }
        else if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.RUN))
        {
            switchRun();
        }
    }

    void switchRun ()
    {
        if (groundedInternalState != null && groundedInternalState is CrouchState)
        {
            groundedInternalState.Exit();
            groundedInternalState = null;
        }

        if (groundedInternalState != null && groundedInternalState is RunState)
        {
            groundedInternalState.Exit();
            groundedInternalState = null;
        }
        else
        {
            setNewInternalState(new RunState ());
        }
    }

    void switchCrouch ()
    {
        if ((groundedInternalState != null && ! (groundedInternalState is RunState)) || groundedInternalState == null)
        {
            if (groundedInternalState == null)
            {
                setNewInternalState(new CrouchState());
            }
            else
            {
                groundedInternalState.Exit();
                groundedInternalState = null;
            }
        }
        
    }

    protected override void onEnter()
    {
        Vector3 currentVelocity = parent.Velocity;
        currentVelocity.y = 0;
        setVelocity(currentVelocity);

        Camera mainCamera = Camera.main;
        DOTween.To(() => capsuleCollider.height, x => capsuleCollider.height = x, 2f, 0.3f);
        mainCamera.transform.DOLocalMoveY(1f, 0.3f);
    }

    protected override void onExit()
    {
        
    }

    void jump ()
    {
        float velocityY = Mathf.Sqrt(parent.JumpHeight * 2f * BurinkeruCharacterController.GRAVITY);
        addVelocity(new Vector3 (0f, velocityY, 0f));

        setNewState(new InAirState ());
    }

    void setNewInternalState(CharacterControllerStateBase newState)
    {
        if (groundedInternalState == null || newState.GetType() != groundedInternalState.GetType())
        {
            if (groundedInternalState != null)
            {
                groundedInternalState.Exit();
            }

            newState.Enter(groundedInternalState, inputManager, parent, capsuleCollider);
            groundedInternalState = newState;
        }
    }

    public override float GetMovementSpeedFactor()
    {
        float result = 1f;

        if (groundedInternalState != null)
        {
            result = groundedInternalState.GetMovementSpeedFactor();
        }

        return result;
    }
}
