using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStabbedState : NPCState
{
    float timer = 0;
    const float duration = 0.25f;

    public override void ApplyForces ()
    {
      
    }

    public override float GetMovementSpeedFactor ()
    {
        return 1f;
    }

    public override void UpdateMovement ()
    {
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            exit ();
        }
    }

    void exit ()
    {
        NPCController.Animator.SetTrigger ("StabbedToGround");

        if (characterController.IsGrounded)
        {
            setNewState (new NPCGroundState ());
        }
        else
        {
            setNewState (new NPCInAirState ());
        }
    }

    protected override void onEnter ()
    {
        timer = 0;
        facePlayer ();
        Vector3 lookDirection = NPCController.transform.forward;
        Vector3 velocity = NPCController.Velocity;
        velocity -= lookDirection * 35f;
        velocity.y = 0f;
        NPCController.SetVelocity (velocity);
        NPCController.Animator.SetTrigger ("Stab");
    }

    protected override void onExit ()
    {

    }

    public override float GetMovementDrag ()
    {
        return 0.01f;
    }
}
