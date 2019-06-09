using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : CharacterControllerStateBase
{
    int jumpCounter = 0;

    public override void ApplyForces()
    {
       
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
    }

    protected override void onEnter()
    {
        
    }

    protected override void onExit()
    {
        
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
