using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInAirState : NPCState
{
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

    }

    protected override void onExit ()
    {
        
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
    }
}
