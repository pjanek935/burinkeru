using DG.Tweening;
using UnityEngine;

public class PlayerCrouchState : PlayerState
{
    const float crouchHeight = 1f;
    const float crouchCameraHeoght = 0.5f;

    public override void ApplyForces()
    {
        
    }

    public override float GetMovementSpeedFactor()
    {
        return CharacterControllerParameters.Instance.MovementSpeedFactorWhileCrouching;
    }

    public override void UpdateMovement()
    {
        
    }

    protected override void onEnter()
    {
        DOTween.To(() => components.CapsuleCollider.height, x => components.CapsuleCollider.height = x, 1f, 0.3f);
        components.Body.transform.DOLocalMoveY(0.5f, 0.3f);

        if (characterController.IsGrounded)
        {
            components.CapsuleCollider.transform.DOLocalMoveY(components.CapsuleCollider.transform.position.y - 0.25f, 0.3f);
        }

        components.RigManager.CurrentRig.SetWalkSpeed(0.8f);
    }

    protected override void onExit()
    {
        DOTween.To(() => components.CapsuleCollider.height, x => components.CapsuleCollider.height = x, 2f, 0.3f);
        components.Body.transform.DOLocalMoveY(1f, 0.3f);
        components.RigManager.CurrentRig.SetWalkSpeed(1f);
    }
}
