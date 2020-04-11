using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCState : CharacterStateBase
{
    public NPCController NPCController
    {
        get { return (NPCController) characterController; }
    }

    public virtual void Uppercut ()
    {
        facePlayer ();
        Vector3 velocity = NPCController.Velocity;
        velocity.y = 14f;
        NPCController.SetVelocity (velocity);
        NPCController.Animator.SetTrigger ("Uppercut");
    }

    public virtual void Stab ()
    {
        setNewState (new NPCStabbedState ());
    }

    protected void facePlayer ()
    {
        Vector3 lookDirection = NPCController.PlayerTransform.position;
        lookDirection.y = NPCController.transform.position.y;
        NPCController.transform.LookAt (lookDirection);
    }

    public override float GetMovementDrag ()
    {
        return 0.02f;
    }
}
