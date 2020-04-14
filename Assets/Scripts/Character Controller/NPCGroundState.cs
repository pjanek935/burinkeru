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

        if (Mathf.Abs (Parent.Velocity.y) < float.Epsilon)
        {
            NPCController.Animator.SetTrigger ("GetUp");
        }
    }

    protected override void onExit ()
    {
        NPCController.Animator.ResetTrigger ("GetUp");
    }
}
