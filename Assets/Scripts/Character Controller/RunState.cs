using DG.Tweening;
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
        Camera camera = Camera.main;
        DOTween.To(() => camera.fieldOfView, x => camera.fieldOfView = x, 70f, 0.45f);
    }

    protected override void onExit()
    {
        Camera camera = Camera.main;
        DOTween.To(() => camera.fieldOfView, x => camera.fieldOfView = x, 60f, 0.45f);
    }
}
