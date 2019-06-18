using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : CharacterControllerStateBase
{
    int jumpCounter = 0;
    Vector3 onEnterPos;

    public override void ApplyForces()
    {
       
    }

    public override float GetMovementSpeedFactor()
    {
        return 1f;
    }

    public override void UpdateMovement()
    {
        float gravity = -BurinkeruCharacterController.GRAVITY * Time.deltaTime;
        addVelocity(new Vector3(0f, gravity, 0));

        if (parent.IsGrounded)
        {
            setNewState(new GroundState());
        }

        if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.JUMP) && jumpCounter == 0)
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
    }

    protected override void onExit()
    {
        Vector3 distance = parent.transform.position - onEnterPos;
        Debug.Log("DY: " + distance.y);

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
        float velocityY = Mathf.Sqrt(parent.JumpHeight * 2f * BurinkeruCharacterController.GRAVITY);
        Vector3 currentVelocity = parent.Velocity;
        currentVelocity.y = velocityY;
        setVelocity (currentVelocity);
    }
}
