using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRunState : PlayerState
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

    float counter = 0f;
    WallRunRaycastResult hitResult;
    Collider collider;
    float velocity = 0;
    CharacterControllerParameters parameters;

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

    public PlayerWallRunState(WallRunRaycastResult hitResult) : base ()
    {
        this.hitResult = hitResult;
        collider = hitResult.ObjectThatWasHit.GetComponent<Collider>();
        IsActive = false;
        parameters = CharacterControllerParameters.Instance;
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
                move (hitResult.RunDirection * velocity * Time.deltaTime);
            }
        }
        else
        {
            stopWallRunning();
        }

        if (! IsActive)
        {
            if (counter >= parameters.WallRunDelay && IsActive == false)
            {
                IsActive = true;
                enter();
            }
        }
        else if (JumpAllowed)
        {
            if (counter >= parameters.WallRunAllowJumpTime && JumpAllowed)
            {
                JumpAllowed = false;
            }
        }

        if (counter >= parameters.WallRunDuration)
        {
            applyGravity();
        }

        counter += Time.deltaTime;
    }

    bool checkIfTouchesWall ()
    {
        Vector3 direction = Vector3.zero;
        bool result = false;

        switch (hitResult.WallRunType)
        {
            case WallRunType.LEFT:

                direction = -characterController.transform.right;

                break;

            case WallRunType.RIGH:

                direction = characterController.transform.right;

                break;
        }

        RaycastHit raycastHit;
        Ray ray = new Ray(characterController.transform.position, direction);

        if (collider.Raycast(ray, out raycastHit, parameters.WallRunRaycastLength))
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
        Vector3 speed = characterController.Velocity;
        speed.y = 0;
        velocity = speed.sqrMagnitude + characterController.GetMovementSpeed ();

        velocity = Mathf.Clamp (velocity, CharacterControllerParameters.Instance.MinWallRunSpeed,
            CharacterControllerParameters.Instance.MaxWallRunSpeed);
    }

    void applyGravity()
    {
        float gravity = -CharacterControllerParameters.Instance.Gravity * Time.deltaTime;
        addVelocity(new Vector3(0f, gravity, 0));
    }

    void enter ()
    {
        Vector3 velocity = characterController.Velocity;
        velocity.y = 0;
        characterController.SetVelocity(velocity);

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

        if (Physics.Raycast(transform.position, direction, out hit, CharacterControllerParameters.Instance.WallRunRaycastLength, LayerMask.GetMask("Default")))
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
