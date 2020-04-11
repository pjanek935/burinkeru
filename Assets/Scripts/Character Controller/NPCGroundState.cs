using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGroundState : NPCState
{
    public override void ApplyForces ()
    {
        if (!characterController.IsGrounded)
        {
            setNewState (new NPCInAirState ());
        }
    }

    public override float GetMovementSpeedFactor ()
    {
        return 1f;
    }

    public override void UpdateMovement ()
    {

    }

    protected override void onEnter ()
    {
        Vector3 velocity = characterController.Velocity;
        velocity.y = 0;
        characterController.SetVelocity (velocity);
    }

    protected override void onExit ()
    {

    }
}
