using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : CharacterControllerStateBase
{
    int jumpCounter = 0;
    Vector3 onEnterPos;
    Vector3 jumpDirection = Vector3.zero;

    public override void ApplyForces()
    {
       
    }

    public override float GetMovementSpeedFactor()
    {
        return CharacterControllerParameters.Instance.MovementSpeedFactorInAir;
    }

    public override void UpdateMovement()
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

        float dot = Vector3.Dot(jumpDirection, deltaPosition);


        if (dot < 0)
        {
            addVelocity(deltaPosition * Mathf.Abs (dot) * Time.deltaTime);
        }

        float gravity = -BurinkeruCharacterController.GRAVITY * Time.deltaTime;
        addVelocity(new Vector3(0f, gravity, 0));

        if (parent.IsGrounded)
        {
            setNewState(new GroundState());
        }

        if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.CROUCH))
        {
            switchCrouch();
        }
        else if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.JUMP) && jumpCounter == 0)
        {
            jump();
        }

        if (parent.transform.position.y > onEnterPos.y)
        {
            onEnterPos = parent.transform.position;
        }
    }

    protected override void onEnter()
    {
        onEnterPos = parent.transform.position;
        jumpDirection = parent.DeltaPosition.normalized;
        jumpDirection.Scale(BurinkeruCharacterController.MovementAxes);
    }

    public override float GetMovementDrag()
    {
        return CharacterControllerParameters.Instance.MovementDragInAir;
    }

    protected override void onExit()
    {
        Vector3 distance = parent.transform.position - onEnterPos;

        if (distance.y < -3.5f)
        {
            components.Head.AnimateHardLand();
        }
        else if (distance.y < - 1f)
        {
            components.Head.AnimateLand();
        }
    }

    void jump()
    {
        jumpCounter++;
        float velocityY = Mathf.Sqrt(CharacterControllerParameters.Instance.DefaultJumpHeight * 2f * BurinkeruCharacterController.GRAVITY);
        Vector3 currentVelocity = parent.Velocity;
        currentVelocity.y = velocityY;
        setVelocity (currentVelocity);
    }
}
