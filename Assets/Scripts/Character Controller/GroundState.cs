using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : CharacterControllerStateBase
{
    public override void ApplyForces()
    {
        if (! parent.IsGrounded)
        {
            setNewState(new InAirState());
        }
    }

    public override void UpdateMovement()
    {
        if (inputManager.IsCommandDown (BurinkeruInputManager.InputCommand.JUMP))
        {
            jump();
        }
    }

    protected override void onEnter()
    {
        Vector3 currentVelocity = parent.Velocity;
        currentVelocity.y = 0;
        setVelocity(currentVelocity);
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
}
