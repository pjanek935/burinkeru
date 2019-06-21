using UnityEngine;

public class GroundState : CharacterControllerStateBase
{
    CharacterControllerStateBase groundedInternalState;

    public override void ApplyForces()
    {
        if (! parent.IsGrounded)
        {
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
        if (groundedInternalState != null && groundedInternalState is SlideState)
        {
            groundedInternalState.UpdateMovement();
        }
        else
        {
            updateDefaultGroundMovement();
        }

        if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.CROUCH))
        {
            switchCrouch();
        }
        else if (inputManager.IsCommandDown (BurinkeruInputManager.InputCommand.JUMP))
        {
            onJumpButtonClicked();
        }
        else if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.RUN))
        {
            switchRun();
        }
    }

    void updateDefaultGroundMovement()
    {
        Vector3 deltaPosition = Vector3.zero;
        Vector3 forwardDirection = parent.transform.forward;
        Vector3 rightDirection = parent.transform.right;
        float movementSpeed = parent.GetMovementSpeed();

        if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.FORWARD))
        {
            deltaPosition += (forwardDirection);
        }
        else if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.BACKWARD))
        {
            deltaPosition -= (forwardDirection);
        }

        if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.RIGHT))
        {
            deltaPosition += (rightDirection);
        }
        else if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.LEFT))
        {
            deltaPosition -= (rightDirection);
        }

        deltaPosition.Normalize();
        deltaPosition *= movementSpeed;
        deltaPosition.Scale(BurinkeruCharacterController.MovementAxes);
        move(deltaPosition * Time.deltaTime);

        if (groundedInternalState != null && parent.DeltaPosition.sqrMagnitude == 0)
        {
            groundedInternalState.Exit();
            groundedInternalState = null;
        }
    }

    void onJumpButtonClicked()
    {
        if (parent.IsCrouching)
        {
            if (groundedInternalState != null && groundedInternalState is SlideState)
            {
                jump();
            }

            parent.ExitCrouch();
        }
        else
        {
            jump();
        }
    }

    void switchRun()
    {
        if (groundedInternalState == null)
        {
            if (parent.IsCrouching)
            {
                exitCrouch();
            }

            setNewInternalState(new RunState());
        }
    }

    protected override void switchCrouch()
    {
        if (parent.IsCrouching)
        {
            exitCrouch();
        }
        else
        {
            enterCrouch();
        }
    }

    void enterCrouch()
    {
        if (groundedInternalState != null && groundedInternalState is RunState)
        {
            slide();
        }

        parent.EnterCrouch();
    }

    void exitCrouch()
    {
        if (groundedInternalState != null && groundedInternalState is SlideState)
        {
            groundedInternalState.Exit();
            groundedInternalState = null;
        }

        parent.ExitCrouch();
    }

    protected override void onEnter()
    {
        Vector3 currentVelocity = parent.Velocity;
        currentVelocity.y = 0;
        setVelocity(currentVelocity);

        switchToSlideIfNeeded ();
    }

    void switchToSlideIfNeeded ()
    {
        if (parent.IsCrouching)
        {
            Vector3 currentVelocity = parent.DeltaPosition;
            currentVelocity.Scale(BurinkeruCharacterController.MovementAxes);
            currentVelocity /= Time.deltaTime;

            if (currentVelocity.sqrMagnitude > 10)
            {
                slide();
            }
        }
    }

    void slide ()
    {
        if (groundedInternalState != null)
        {
            groundedInternalState.Exit();
        }
        
        SlideState slideState = new SlideState();
        setReferences(slideState);
        slideState.OnExitSlideStateRequested += onExitSlideStateRequested;
        slideState.Enter(null, inputManager, parent, components);
        groundedInternalState = slideState;
    }

    void onExitSlideStateRequested ()
    {
        if (groundedInternalState != null && groundedInternalState is SlideState)
        {
            groundedInternalState.Exit();
            groundedInternalState = null;
        }
    }

    protected override void onExit()
    {
        
    }

    public override float GetMovementDrag()
    {
        float result = CharacterControllerParameters.Instance.MovementDragOnGround;

        if (groundedInternalState != null)
        {
            result = groundedInternalState.GetMovementDrag();
        }

        return result;
    }

    void jump ()
    {
        float velocityY = Mathf.Sqrt(CharacterControllerParameters.Instance.DefaultJumpHeight * 2f * BurinkeruCharacterController.GRAVITY);
        addVelocity(new Vector3 (0f, velocityY, 0f));
        Vector3 horizontalVelocity = parent.DeltaPosition;
        horizontalVelocity.Scale(BurinkeruCharacterController.MovementAxes);
        horizontalVelocity /= Time.deltaTime;
        addVelocity(horizontalVelocity);
        components.Head.AnimateJump();

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

            newState.Enter(groundedInternalState, inputManager, parent, components);
            groundedInternalState = newState;
        }
    }

    public override float GetMovementSpeedFactor()
    {
        float result = 1f;

        if (groundedInternalState != null)
        {
            result *= groundedInternalState.GetMovementSpeedFactor();
        }

        return result;
    }
}
