using UnityEngine;

public class GroundState : CharacterControllerStateBase
{
    CharacterControllerStateBase groundedInternalState;

    public override void ApplyForces()
    {
        if (! characterController.IsGrounded)
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

    public bool IsSliding ()
    {
        bool result = false;

        if (groundedInternalState != null && groundedInternalState is SlideState)
        {
            result = true;
        }

        return result;
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
        else if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.RUN) && ! characterController.IsBlinking)
        {
            switchRun();
        }
    }

    void updateDefaultGroundMovement()
    {
        Vector3 deltaPosition = Vector3.zero;
        Vector3 forwardDirection = characterController.transform.forward;
        Vector3 rightDirection = characterController.transform.right;
        float movementSpeed = characterController.GetMovementSpeed();

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

        if (groundedInternalState != null && characterController.DeltaPosition.sqrMagnitude == 0)
        {
            groundedInternalState.Exit();
            groundedInternalState = null;
        }

        if ( !(groundedInternalState is SlideState))
        {
            float magnitude = deltaPosition.magnitude;

            if (magnitude != 0)
            {
                components.RigManager.CurrentRig.SetWalk(true);
            }
            else
            {
                components.RigManager.CurrentRig.SetWalk(false);
            }
        }
    }

    void onJumpButtonClicked()
    {
        if (characterController.IsCrouching)
        {
            if (groundedInternalState != null && groundedInternalState is SlideState)
            {
                jump();
            }

            characterController.ExitCrouch();
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
            if (characterController.IsCrouching)
            {
                exitCrouch();
            }

            setNewInternalState(new RunState());
        }
    }

    protected override void switchCrouch()
    {
        if (characterController.IsCrouching)
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

        characterController.EnterCrouch();
    }

    void exitCrouch()
    {
        if (groundedInternalState != null && groundedInternalState is SlideState)
        {
            groundedInternalState.Exit();
            groundedInternalState = null;
        }

        characterController.ExitCrouch();
    }

    protected override void onEnter()
    {
        Vector3 currentVelocity = characterController.Velocity;
        currentVelocity.y = 0;
        setVelocity(currentVelocity);

        switchToSlideIfNeeded ();
    }

    void switchToSlideIfNeeded ()
    {
        if (characterController.IsCrouching)
        {
            Vector3 currentVelocity = characterController.DeltaPosition;
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
        slideState.OnExitSlideStateRequested += onExitSlideStateRequested;
        slideState.Enter(null, inputManager, characterController, components);
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
       components.RigManager.CurrentRig.SetWalk(false);
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
        float velocityY = Mathf.Sqrt(CharacterControllerParameters.Instance.DefaultJumpHeight * 2f * CharacterControllerParameters.Instance.Gravity);
        addVelocity(new Vector3 (0f, velocityY, 0f));
        Vector3 horizontalVelocity = characterController.DeltaPosition;
        horizontalVelocity.Scale(BurinkeruCharacterController.MovementAxes);
        horizontalVelocity /= Time.deltaTime;
        addVelocity(horizontalVelocity);
        components.Head.AnimateJump();
        Vector3 velocity = characterController.Velocity;
        velocity.Scale(BurinkeruCharacterController.MovementAxes);

        if (velocity.magnitude > CharacterControllerParameters.Instance.MaxInAirHorizontalVelocity)
        {
            velocity = CharacterControllerParameters.Instance.MaxInAirHorizontalVelocity * velocity.normalized;
            velocity.y = characterController.Velocity.y;
            setVelocity(velocity);
        }

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

            newState.Enter(groundedInternalState, inputManager, characterController, components);
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
