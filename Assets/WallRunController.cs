using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunRaycastResult
{
    public bool Success = false;
    public float Distance = -1f;
    public GameObject ObjectThatWasHit;
    public WallRunType WallRunType;
    public RaycastHit Hit;
    public Vector3 Direction;
}

public enum WallRunType
{
    NONE, LEFT, RIGH
}

public class WallRunController : MonoBehaviour
{
    [SerializeField] BurinkeruCharacterController characterController;

    const float RAYCAST_LENGTH = 10f;

    BurinkeruInputManager inputManager;
    int layerMaskToCheckForWalls = 0;
    WallRunRaycastResult hitResult = null;

    public bool IsWallRunning
    {
        get;
        private set;
    }

    private void Start()
    {
        inputManager = BurinkeruInputManager.Instance;
        layerMaskToCheckForWalls = LayerMask.GetMask("Default");
    }
    private void Update()
    {
        if (inputManager.IsCommandDown(BurinkeruInputManager.InputCommand.JUMP))
        {
            tryToInitWallRun();
        }

        if (inputManager.IsCommandUp (BurinkeruInputManager.InputCommand.JUMP))
        {
            stopWallRunning();
        }

        updateWallRun();
    }

    void updateWallRun ()
    {
        if (IsWallRunning)
        {
            
        }
    }

    void tryToInitWallRun ()
    {
        if (canInitWallRun ())
        {
            WallRunRaycastResult raycastResult = raycastWalls();

            if (raycastResult.Success)
            {
                initWallRun(raycastResult);
            }
        }
    }

    void initWallRun (WallRunRaycastResult raycastResult)
    {
        IsWallRunning = true;
        this.hitResult = raycastResult;
        Vector3 velocity = characterController.Velocity;
        velocity.y = 0;
        characterController.SetVelocity(velocity);
    }

    void stopWallRunning ()
    {
        if (IsWallRunning)
        {
            //TODO
            IsWallRunning = false;
        }
    }

    WallRunRaycastResult raycastWalls ()
    {
        WallRunRaycastResult result = new WallRunRaycastResult();
        GameObject rightWall;
        GameObject leftWall;
        RaycastHit leftHit;
        RaycastHit rightHit;
        float distFromRight = raycastInDirection(transform.right, out rightWall, out leftHit);
        float distFromLeft = raycastInDirection(-transform.right, out leftWall, out rightHit);

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

    float raycastInDirection (Vector3 direction, out GameObject objectThatWasHit, out RaycastHit hit)
    {
        float result = -1f;
        objectThatWasHit = null;

        if (Physics.Raycast(transform.position, direction, out hit, RAYCAST_LENGTH, layerMaskToCheckForWalls))
        {

            result = hit.distance;
            objectThatWasHit = hit.collider.gameObject;
            Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow);
        }

        return result;
    }

    bool canInitWallRun ()
    {
        return (! IsWallRunning) && (! characterController.IsGrounded);
    }
}
