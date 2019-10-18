using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : CharacterControllerStateBase
{
    int jumpCounter = 0;
    Vector3 onEnterPos;
    Vector3 jumpDirection = Vector3.zero;
    WallRunState wallRunState = null;

    bool wallRunToLastColliderAllowed = true;
    Collider lastWallRunCollider = null;

    public override void ApplyForces()
    {
        
    }

    public override float GetMovementSpeedFactor()
    {
        return CharacterControllerParameters.Instance.MovementSpeedFactorInAir;
    }

    public override void UpdateMovement()
    {
        move();
        applyGravity();
        updateState();

        if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.CROUCH))
        {
            switchCrouch();
        }
        else if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.JUMP))
        {
            if (wallRunState == null)
            {
                bool wallRunSuccess = tryToWallRun();

                if (! wallRunSuccess)
                {
                    tryToJump();
                }
            }
            else
            {
                bool jumped = tryToJump();

                if (jumped)
                {
                    stopWallRun();
                }
            }
        }
        else if (inputManager.IsCommandUp (BurinkeruInputManager.InputCommand.JUMP))
        {
            if (wallRunState != null)
            {
                bool isJumpAllowed = wallRunState.JumpAllowed;
                stopWallRun();

                if (isJumpAllowed)
                {
                    tryToJump();
                }
            }
        }

        if (wallRunState != null)
        {
            wallRunState.UpdateMovement();
        }
    }

    void stopWallRun ()
    {
        if (wallRunState != null)
        {
            if (wallRunState.IsActive)
            {
                jumpCounter = 0;
            }

            wallRunState.Exit();
            wallRunState = null;
            wallRunToLastColliderAllowed = false;
            parent.StartCoroutine(wallRunDelay ());
        }
    }

    bool tryToWallRun()
    {
        bool success = false;
        WallRunState.WallRunRaycastResult result = WallRunState.RaycastWalls(parent.transform);

        if (result.Success)
        {
            if (lastWallRunCollider == null ||
                (lastWallRunCollider != result.Hit.collider) ||
                (lastWallRunCollider == result.Hit.collider && wallRunToLastColliderAllowed))
            {
                lastWallRunCollider = result.Hit.collider;

                Vector3 lookDirection = parent.GetLookDirection();
                float dot = Vector3.Dot(lookDirection, result.Direction);
                success = true;
                Vector3 v = parent.GetLookDirection();
                Vector3 n = result.Hit.normal;
                Vector3 vtn = Vector3.Cross(v, n);
                Vector3 res = Vector3.Cross(n, vtn);
                res.y *= 0.1f;

                result.RunDirection = res;

                wallRunState = new WallRunState(result);
                wallRunState.Enter(this, inputManager, parent, components);
                wallRunState.RequestExit += onExitRequested;

                Debug.DrawRay(result.Hit.point, res * 10, Color.magenta, 1f);
            }
        }

        return success;
    }

    IEnumerator wallRunDelay ()
    {
        yield return new WaitForSeconds(0.5f);

        wallRunToLastColliderAllowed = true;
    }

    void onExitRequested ()
    {
        stopWallRun();
    }

    bool tryToJump ()
    {
        bool result = false;

        if (jumpCounter < 1)
        {
            jump();
            result = true;
        }

        return result;
    }

    void clampHorizontalVelocity ()
    {
        Vector3 velocity = parent.Velocity;

        if (velocity.magnitude > CharacterControllerParameters.Instance.MaxInAirHorizontalVelocity)
        {
            velocity = CharacterControllerParameters.Instance.MaxInAirHorizontalVelocity * velocity.normalized;
            velocity.y = parent.Velocity.y;
            setVelocity(velocity);
        }
    }

    void move ()
    {
        if (wallRunState == null)
        {
            Vector3 deltaPosition = getMoveDirection();
            float movementSpeed = parent.GetMovementSpeed();
            deltaPosition *= movementSpeed;
            move(deltaPosition * Time.deltaTime);
        }
       
        clampHorizontalVelocity();
    }

    Vector3 getMoveDirection ()
    {
        Vector3 forwardDirection = parent.transform.forward;
        Vector3 rightDirection = parent.transform.right;
        Vector3 deltaPosition = Vector3.zero;

        if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.FORWARD))
        {
            deltaPosition += (forwardDirection);
        }
        else if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.BACKWARD))
        {
            deltaPosition -= (forwardDirection);
        }

        if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.RIGHT))
        {
            deltaPosition += (rightDirection);
        }
        else if (inputManager.IsCommandPressed(BurinkeruInputManager.InputCommand.LEFT))
        {
            deltaPosition -= (rightDirection);
        }

        deltaPosition.Normalize();
        deltaPosition.Scale(BurinkeruCharacterController.MovementAxes);

        return deltaPosition;
    }

    void updateState ()
    {
        if (parent.IsGrounded)
        {
            setNewState(new GroundState());
        }

        if (parent.transform.position.y > onEnterPos.y)
        {
            onEnterPos = parent.transform.position;
        }
    }

    void applyGravity ()
    {
        if (! parent.IsBlinking && wallRunState == null)
        {
            float gravity = -BurinkeruCharacterController.GRAVITY * Time.deltaTime;
            addVelocity(new Vector3(0f, gravity, 0));
        }
    }

    protected override void onEnter()
    {
        onEnterPos = parent.transform.position;
        jumpDirection = parent.DeltaPosition.normalized;
        jumpDirection.Scale(BurinkeruCharacterController.MovementAxes);

        if (components.RigManager.CurrentRig != null)
        {
            components.RigManager.CurrentRig.SetInAir(true);
        }
    }

    public override float GetMovementDrag()
    {
        return CharacterControllerParameters.Instance.MovementDragInAir;
    }

    protected override void onExit()
    {
        stopWallRun();

        Vector3 distance = parent.transform.position - onEnterPos;

        if (distance.y < -3.5f)
        {
            components.Head.AnimateHardLand();
        }
        else if (distance.y < - 1f)
        {
            components.Head.AnimateLand();
        }

        components.RigManager.CurrentRig.SetInAir(false);
    }

    void jump()
    {
        jumpCounter++;
        float velocityY = Mathf.Sqrt(CharacterControllerParameters.Instance.DefaultJumpHeight * 2f * BurinkeruCharacterController.GRAVITY);
        Vector3 currentVelocity = parent.Velocity;
        currentVelocity.y = velocityY;
        setVelocity (currentVelocity);
    }
}
