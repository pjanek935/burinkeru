using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerParameters : MonoBehaviour
{
    [SerializeField, Range (1, 50)] float gravity = 30f;

    [SerializeField, Range (0, 10)] float defaultMoveSpeed = 5f;
    [SerializeField, Range (0, 10)] float defaultJumpHeight = 2f;

    [SerializeField, Range(0, 1f)] float movementSpeedFactorOnGround = 1f;
    [SerializeField, Range(0, 1f)] float movementSpeedFactorInAir = 0.8f;
    [SerializeField, Range(1f, 3f)] float movementSpeedFactorWhileRunning = 1.5f;
    [SerializeField, Range(0f, 1f)] float movementSpeedFactorWhileCrouching = 1.5f;
    [SerializeField, Range(0f, 1f)] float movementSpeedFactorWhileSliding = 0.1f;

    [SerializeField, Range (0f, 1f)] float movementDragOnGround = 0.05f;
    [SerializeField, Range(0f, 1f)] float maxMovementDragWhileSliding = 0.1f;
    [SerializeField, Range(0f, 0.001f)] float movementDragWhileSlidingDelta = 0.0004f;
    [SerializeField, Range(0f, 1f)] float movementDragInAir= 0.01f;

    [SerializeField, Range(1f, 50f)] float slideMagnitude = 15f;

    [SerializeField, Range(1f, 20f)] float maxInAirHorizontalVelocity = 8f;

    [SerializeField, Range(0, 3)] int maxJumpsInAir = 1;

    [SerializeField, Range(0f, 2f)] float wallRunRayCastLength = 0.85f;
    [SerializeField, Range(0f, 1f)] float wallRunDelay = 0.1f;
    [SerializeField, Range(0f, 1f)] float wallRunAllowJumpTime = 0.5f;
    [SerializeField, Range(0f, 5f)] float wallRunDuration = 1f;
    [SerializeField, Range (0f, 50f)] float minWallRunSpeed = 3f;
    [SerializeField, Range (0f, 50f)] float maxWallRunSpeed = 5f;

    [SerializeField, Range (50f, 1000f)] float blinkingSpeed = 1f;
    [SerializeField, Range (1, 5)] int maxBlinksInAir = 2;
    [SerializeField, Range (0.1f, 1f)] float blinkingDrag = 0.85f;

    static CharacterControllerParameters instance = null;

    public float Gravity
    {
        get { return gravity; }
    }

    public float DefaultMoveSpeed
    {
        get { return defaultMoveSpeed / (Time.timeScale); }
    }

    public float DefaultJumpHeight
    {
        get { return defaultJumpHeight; }
    }

    public float MovementSpeedFactorOnGround
    {
        get { return movementSpeedFactorOnGround; }
    }

    public float MovementSpeedFactorInAir
    {
        get { return movementSpeedFactorInAir; }
    }

    public float MovementSpeedFactorWhileRunning
    {
        get { return movementSpeedFactorWhileRunning; }
    }

    public float MovementSpeedFactorWhileCrouching
    {
        get { return movementSpeedFactorWhileCrouching; }
    }

    public float MovementSpeedFactorWhileSliding
    {
        get { return movementSpeedFactorWhileSliding; }
    }

    public float MovementDragOnGround
    {
        get { return movementDragOnGround; }
    }

    public float MaxMovementDragWhileSliding
    {
        get { return maxMovementDragWhileSliding; }
    }

    public float MovementDragWhileSlidingDelta
    {
        get { return movementDragWhileSlidingDelta; }
    }

    public float MovementDragInAir
    {
        get { return movementDragInAir; }
    }

    public float SlideMagnitude
    {
        get { return slideMagnitude; }
    }

    public float MaxInAirHorizontalVelocity
    {
        get { return maxInAirHorizontalVelocity; }
    }

    public float WallRunDuration
    {
        get { return wallRunDuration; }
    }

    public float WallRunRaycastLength
    {
        get { return wallRunRayCastLength; }
    }

    public float MinWallRunSpeed
    {
        get { return minWallRunSpeed; }
    }

    public float MaxWallRunSpeed
    {
        get { return maxWallRunSpeed; }
    }

    public int MaxJumpsInAir
    {
        get { return maxJumpsInAir; }
    }

    public float WallRunDelay
    {
        get { return wallRunDelay; }
    }

    public float WallRunAllowJumpTime
    {
        get { return wallRunAllowJumpTime; }
    }

    public float BlinkingSpeed
    {
        get { return blinkingSpeed; }
    }

    public int MaxBlinksInAir
    {
        get { return maxBlinksInAir; }
    }

    public float BlinkingDrag
    {
        get { return blinkingDrag; }
    }

    public static bool IsInstanceNull ()
    {
        return instance == null;
    }

    public static CharacterControllerParameters Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
