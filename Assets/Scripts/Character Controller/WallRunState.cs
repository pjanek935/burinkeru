using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunState : CharacterControllerStateBase
{
    public delegate void RequestExitEventHandler ();
    public event RequestExitEventHandler RequestExit;

    public enum WallRunType
    {
        NONE, LEFT, RIGH
    }

    public class WallRunRaycastResult
    {
        public bool Success = false;
        public float Distance = -1f;
        public GameObject ObjectThatWasHit;
        public WallRunType WallRunType;
        public RaycastHit Hit;
        public Vector3 Direction;
        public Vector3 RunDirection;
    }

    const float RAYCAST_LENGTH = 1f;
    const float START_DELAY = 0.1f;
    const float ALLOW_JUMP_DELAY = 0.5f;
    float counter = 0f;
    WallRunRaycastResult hitResult;
    Collider collider;

    public bool IsActive
    {
        get;
        private set;
    }

    public bool JumpAllowed
    {
        get;
        private set;
    }

    public WallRunState(WallRunRaycastResult hitResult) : base ()
    {
        this.hitResult = hitResult;
        collider = hitResult.ObjectThatWasHit.GetComponent<Collider>();
        IsActive = false;
    }

    public override void ApplyForces()
    {
       
    }

    public override float GetMovementSpeedFactor()
    {
        return 1f;
    }

    public override void UpdateMovement()
    {
        if (checkIfTouchesWall())
        {
            if (IsActive)
            {
                move (hitResult.RunDirection * 10f * Time.deltaTime);
            }
        }
        else
        {
            stopWallRunning();
        }

        if (! IsActive)
        {
            counter += Time.deltaTime;

            if (counter >= START_DELAY)
            {
                IsActive = true;
                enter();
            }
        }
        else if (JumpAllowed)
        {
            counter += Time.deltaTime;

            if (counter >= ALLOW_JUMP_DELAY)
            {
                JumpAllowed = false;
            }
        }
    }

    bool checkIfTouchesWall ()
    {
        Vector3 direction = Vector3.zero;
        bool result = false;

        switch (hitResult.WallRunType)
        {
            case WallRunType.LEFT:

                direction = -parent.transform.right;

                break;

            case WallRunType.RIGH:

                direction = parent.transform.right;

                break;
        }

        RaycastHit raycastHit;
        Ray ray = new Ray(parent.transform.position, direction);

        if (collider.Raycast(ray, out raycastHit, RAYCAST_LENGTH))
        {
            result = true;
        }

        return result;
    }

    protected override void onEnter()
    {
        IsActive = false;
        JumpAllowed = true;
        counter = 0f;
    }

    void enter ()
    {
        Vector3 velocity = parent.Velocity;
        velocity.y = 0;
        parent.SetVelocity(velocity);

        switch (hitResult.WallRunType)
        {
            case WallRunType.LEFT:

                components.SpineAnimationController.WallRunLeft();

                break;

            case WallRunType.RIGH:

                components.SpineAnimationController.WallRunRight();

                break;
        }

        components.RigManager.CurrentRig.SetWalk(true);
    }

    protected override void onExit()
    {
        components.RigManager.CurrentRig.SetWalk(false);
        components.SpineAnimationController.Default();
    }

    public static WallRunRaycastResult RaycastWalls (Transform transform)
    {
        WallRunRaycastResult result = new WallRunRaycastResult();
        GameObject rightWall;
        GameObject leftWall;
        RaycastHit leftHit;
        RaycastHit rightHit;
        float distFromRight = raycastInDirection(transform, transform.right, out rightWall, out rightHit);
        float distFromLeft = raycastInDirection(transform, -transform.right, out leftWall, out leftHit);

        if (distFromLeft > 0 && distFromLeft > distFromRight)
        {
            result.Success = true;
            result.WallRunType = WallRunType.LEFT;
            result.ObjectThatWasHit = leftWall;
            result.Distance = distFromLeft;
            result.Hit = leftHit;
            result.Direction = (leftHit.point - transform.position).normalized;
        }
        else if (distFromRight > 0 && distFromRight > distFromLeft)
        {
            result.Success = true;
            result.WallRunType = WallRunType.RIGH;
            result.ObjectThatWasHit = rightWall;
            result.Distance = distFromRight;
            result.Hit = rightHit;
            result.Direction = (rightHit.point - transform.position).normalized;
        }

        return result;
    }

    private static float raycastInDirection(Transform transform, Vector3 direction, out GameObject objectThatWasHit, out RaycastHit hit)
    {
        float result = -1f;
        objectThatWasHit = null;

        if (Physics.Raycast(transform.position, direction, out hit, RAYCAST_LENGTH, LayerMask.GetMask("Default")))
        {
            result = hit.distance;
            objectThatWasHit = hit.collider.gameObject;
            Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow, 1f);
        }

        return result;
    }

    void stopWallRunning()
    {
        RequestExit?.Invoke();
    }
}
