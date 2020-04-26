using UnityEngine;

public class PlayerGroundState : PlayerState
{
    PlayerState groundedInternalState;

    public override void ApplyForces()
    {
        if (! characterController.IsGrounded)
        {
            setNewState(new PlayerInAirState());
        }
    }

    protected override void setNewState(CharacterStateBase newState)
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

        if (groundedInternalState != null && groundedInternalState is PlayerSlideState)
        {
            result = true;
        }

        return result;
    }

    public override void UpdateMovement()
    {
        BurinkeruCharacterController characterController = (BurinkeruCharacterController) this.characterController;

        if (characterController == null)
        {
            return;
        }

        if (groundedInternalState != null && groundedInternalState is PlayerSlideState)
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

        if ( !(groundedInternalState is PlayerSlideState))
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
        BurinkeruCharacterController characterController = (BurinkeruCharacterController) this.characterController;

        if (characterController == null)
        {
            return;
        }

        if (characterController.IsCrouching)
        {
            if (groundedInternalState != null && groundedInternalState is PlayerSlideState)
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
        BurinkeruCharacterController characterController = (BurinkeruCharacterController) this.characterController;

        if (characterController == null)
        {
            return;
        }

        if (groundedInternalState == null)
        {
            if (characterController.IsCrouching)
            {
                exitCrouch();
            }

            setNewInternalState(new PlayerRunState());
        }
    }

    protected override void switchCrouch()
    {
        BurinkeruCharacterController characterController = (BurinkeruCharacterController) this.characterController;

        if (characterController == null)
        {
            return;
        }
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
        BurinkeruCharacterController characterController = (BurinkeruCharacterController) this.characterController;

        if (characterController == null)
        {
            return;
        }

        if (groundedInternalState != null && groundedInternalState is PlayerRunState)
        {
            slide();
        }

        characterController.EnterCrouch();
    }

    void exitCrouch()
    {
        BurinkeruCharacterController characterController = (BurinkeruCharacterController) this.characterController;

        if (characterController == null)
        {
            return;
        }

        if (groundedInternalState != null && groundedInternalState is PlayerSlideState)
        {
            groundedInternalState.Exit();
            groundedInternalState = null;
        }

        characterController.ExitCrouch();
    }

    protected override void onEnter()
    {
        BurinkeruCharacterController characterController = (BurinkeruCharacterController) this.characterController;

        if (characterController == null)
        {
            return;
        }

        Vector3 currentVelocity = characterController.Velocity;
        currentVelocity.y = 0;
        setVelocity(currentVelocity);

        switchToSlideIfNeeded ();
    }

    void switchToSlideIfNeeded ()
    {
        BurinkeruCharacterController characterController = (BurinkeruCharacterController) this.characterController;

        if (characterController == null)
        {
            return;
        }

        if (characterController.IsCrouching)
        {
            float enterTime = characterController.CrouchState.EnterTime;
            float currentTime = Time.unscaledTime;
            float timeDiff = currentTime - enterTime;

            Vector3 currentVelocity = characterController.DeltaPosition;
            currentVelocity.Scale(BurinkeruCharacterController.MovementAxes);
            currentVelocity /= Time.deltaTime;

            if (currentVelocity.sqrMagnitude > 10 && timeDiff < 0.5f)
            {
                slide();
            }
        }
    }

    void slide ()
    {
        BurinkeruCharacterController characterController = (BurinkeruCharacterController) this.characterController;

        if (characterController == null)
        {
            return;
        }

        if (groundedInternalState != null)
        {
            groundedInternalState.Exit();
        }
        
        PlayerSlideState slideState = new PlayerSlideState();
        slideState.OnExitSlideStateRequested += onExitSlideStateRequested;
        slideState.Enter(inputManager, characterController, components);
        groundedInternalState = slideState;
    }

    void onExitSlideStateRequested ()
    {
        if (groundedInternalState != null && groundedInternalState is PlayerSlideState)
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

        setNewState(new PlayerInAirState ());
    }

    void setNewInternalState(PlayerState newState)
    {
        BurinkeruCharacterController characterController = (BurinkeruCharacterController) this.characterController;

        if (characterController == null)
        {
            return;
        }

        if (groundedInternalState == null || newState.GetType() != groundedInternalState.GetType())
        {
            if (groundedInternalState != null)
            {
                groundedInternalState.Exit();
            }

            newState.Enter(inputManager, characterController, components);
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
