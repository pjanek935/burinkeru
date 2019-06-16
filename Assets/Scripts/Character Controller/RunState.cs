using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : CharacterControllerStateBase
{
    public override void ApplyForces()
    {
       
    }

    public override float GetMovementSpeedFactor()
    {
        return 1.5f;
    }

    public override void UpdateMovement()
    {
        
    }

    protected override void onEnter()
    {
        Camera.main.fieldOfView = 75;
    }

    protected override void onExit()
    {
        Camera.main.fieldOfView = 60;
    }
}
