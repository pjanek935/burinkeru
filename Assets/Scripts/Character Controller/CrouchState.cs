using DG.Tweening;
using UnityEngine;

public class CrouchState : CharacterControllerStateBase
{
    const float crouchHeight = 1f;
    const float crouchCameraHeoght = 0.5f;

    public override void ApplyForces()
    {
        
    }

    public override float GetMovementSpeedFactor()
    {
        return 0.5f;
    }

    public override void UpdateMovement()
    {
        
    }

    protected override void onEnter()
    {
        Debug.Log("Enter crouch state");

        Camera mainCamera = Camera.main;
        DOTween.To(() => capsuleCollider.height, x => capsuleCollider.height = x, 1f, 0.3f);
        capsuleCollider.transform.DOLocalMoveY(capsuleCollider.transform.position.y - 0.25f, 0.3f);
        mainCamera.transform.DOLocalMoveY(0.5f, 0.3f);
    }

    public void ExitImmediate ()
    {
        //Debug.Log("Exit crouch state");
        //Camera mainCamera = Camera.main;
        //capsuleCollider.height = 2f;
        //mainCamera.transform.localPosition = new Vector3(mainCamera.transform.localPosition.x, 1f, mainCamera.transform.localPosition.z);
    }

    protected override void onExit()
    {
        Debug.Log("Exit crouch state");
        Camera mainCamera = Camera.main;
        DOTween.To(() => capsuleCollider.height, x => capsuleCollider.height = x, 2f, 0.3f);
        mainCamera.transform.DOLocalMoveY(1f, 0.3f);
    }
}
