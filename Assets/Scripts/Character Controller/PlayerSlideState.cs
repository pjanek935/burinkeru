using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerState
{
    public delegate void SlideStateExitRequestEventHandler();
    public event SlideStateExitRequestEventHandler OnExitSlideStateRequested;

    float currentDrag = 0f;

    protected override void onEnter()
    {
        currentDrag = 0f;
        Vector3 deltaPos = characterController.DeltaPosition;
        deltaPos.Scale(BurinkeruCharacterController.MovementAxes);
        deltaPos.Normalize();
        deltaPos *= CharacterControllerParameters.Instance.SlideMagnitude;
        setVelocity(deltaPos);

        components.RigManager.CurrentRig.SetSlide(true);
    }

    protected override void onExit()
    {
        components.RigManager.CurrentRig.SetSlide(false);
    }

    public override float GetMovementDrag()
    {
        return currentDrag;
    }

    public override void UpdateMovement()
    {
        updateDrag();
        move();
        exitSlideStateIfNeeded();
    }

    void move ()
    {
        Vector3 currentMovementDirection = characterController.DeltaPosition;
        currentMovementDirection.Normalize();

        Vector3 deltaMove = Vector3.zero;
        Vector3 forwardDirection = characterController.transform.forward;
        Vector3 rightDirection = characterController.transform.right;
        float movementSpeed = characterController.GetMovementSpeed();

        if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.FORWARD))
        {
            deltaMove += (forwardDirection);
        }
        else if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.BACKWARD))
        {
            deltaMove -= (forwardDirection);
        }

        if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.RIGHT))
        {
            deltaMove += (rightDirection);
        }
        else if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.LEFT))
        {
            deltaMove -= (rightDirection);
        }

        deltaMove.Normalize();
        deltaMove.Scale(BurinkeruCharacterController.MovementAxes);
        float dot = Vector3.Dot(currentMovementDirection, deltaMove);
        deltaMove = (deltaMove * movementSpeed) - (deltaMove * movementSpeed * dot);
        addVelocity(deltaMove);
    }

    void updateDrag ()
    {
        if (currentDrag < CharacterControllerParameters.Instance.MaxMovementDragWhileSliding)
        {
            currentDrag += CharacterControllerParameters.Instance.MovementDragWhileSlidingDelta;
        }
        else
        {
            currentDrag = CharacterControllerParameters.Instance.MaxMovementDragWhileSliding;
        }
    }

    void exitSlideStateIfNeeded ()
    {
        Vector3 velocity = characterController.Velocity;

        if (velocity.sqrMagnitude < 1)
        {
            OnExitSlideStateRequested?.Invoke();
        }
    }

    public override void ApplyForces()
    {
        
    }

    public override float GetMovementSpeedFactor()
    {
        return CharacterControllerParameters.Instance.MovementSpeedFactorWhileSliding;
    }
}
