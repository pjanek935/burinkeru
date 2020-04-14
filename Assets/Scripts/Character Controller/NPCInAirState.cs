using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInAirState : NPCState
{
    bool isGoingUp = false;

    public override void ApplyForces ()
    {
        
    }

    public override float GetMovementSpeedFactor ()
    {
        return 1f;
    }

    public override void UpdateMovement ()
    {
        applyGravity ();
        updateState ();
    }

    protected override void onEnter ()
    {
        if (Parent.Velocity.y < 0)
        {
            isGoingUp = false;
        }
        else
        {
            isGoingUp = true;
        }

        NPCController.Animator.ResetTrigger ("FallDown");
    }

    protected override void onExit ()
    {
        NPCController.Animator.ResetTrigger ("FallDown");
    }

    void applyGravity ()
    {
        float gravity = -CharacterControllerParameters.Instance.Gravity * Time.deltaTime;
        addVelocity (new Vector3 (0f, gravity, 0));
    }

    void updateState ()
    {
        if (characterController.IsGrounded)
        {
            setNewState (new NPCGroundState ());
        }

        if (Parent.Velocity.y < 5 && isGoingUp)
        {
            isGoingUp = false;
            Animator.SetTrigger ("FallDown");
        }
        else if (Parent.Velocity.y >= 5 && ! isGoingUp)
        {
            isGoingUp = true;
        }
    }
}
