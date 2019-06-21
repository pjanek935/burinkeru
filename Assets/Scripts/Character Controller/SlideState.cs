using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : CharacterControllerStateBase
{
    public delegate void SlideStateExitRequestEventHandler();
    public event SlideStateExitRequestEventHandler OnExitSlideStateRequested;

    float currentDrag = 0f;

    protected override void onEnter()
    {
        currentDrag = 0f;
        Vector3 deltaPos = parent.DeltaPosition;
        deltaPos.Scale(BurinkeruCharacterController.MovementAxes);
        deltaPos.Normalize();
        deltaPos *= CharacterControllerParameters.Instance.SlideMagnitude;
        setVelocity(deltaPos);
    }

    protected override void onExit()
    {

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
        Vector3 currentMovementDirection = parent.DeltaPosition;
        currentMovementDirection.Normalize();

        Vector3 deltaMove = Vector3.zero;
        Vector3 forwardDirection = parent.transform.forward;
        Vector3 rightDirection = parent.transform.right;
        float movementSpeed = parent.GetMovementSpeed();

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
        Vector3 velocity = parent.Velocity;

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
