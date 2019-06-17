using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CapsuleCollider))]
public class BurinkeruCharacterController : MonoBehaviour
{
    [SerializeField] float defaultMovementSpeed = 5f;
    [SerializeField] float defaultJumpHeight = 10f;

    CapsuleCollider capsuleCollider;
    BurinkeruInputManager inputManager;

    CharacterControllerStateBase mainMovementState;
    CrouchState crouchState;

    static Vector3 movementAxes = new Vector3 (1f, 0f, 1f);
    public const float GRAVITY = 9.8f;
    const float heightOffset = 0.1f;

    Vector3 velocity = Vector3.zero;

    public Vector3 DeltaPosition
    {
        get;
        private set;
    }

    public Vector3 Velocity
    {
        get { return velocity; }
    }

    public float MovementSpeed
    {
        get { return defaultMovementSpeed; }
    }

    public float JumpHeight
    {
        get { return defaultJumpHeight; }
    }

    public bool IsGrounded
    {
        get;
        private set;
    }

    public bool IsCrouching
    {
        get { return crouchState != null; }
    }

    void setNewState <T> () where T : CharacterControllerStateBase
    {
        if (mainMovementState == null || typeof (T) != mainMovementState.GetType ())
        {
            CharacterControllerStateBase newState = (T)Activator.CreateInstance(typeof(T));

            if (mainMovementState != null)
            {
                exitState(mainMovementState);
            }

            enterState(newState);
            mainMovementState = newState;
        }
    }

    void setNewState (CharacterControllerStateBase newState)
    {
        if (mainMovementState == null || newState.GetType () != mainMovementState.GetType())
        {
            if (mainMovementState != null)
            {
                exitState(mainMovementState);
            }

            enterState(newState);
            mainMovementState = newState;
        }
    }

    void enterState (CharacterControllerStateBase state)
    {
        state.OnSetNewStateRequested += setNewState;
        state.OnAddVelocityRequested += addVelocity;
        state.OnSetVelocityRequested += setVelocity;
        state.Enter(mainMovementState, inputManager, this, capsuleCollider);
    }

    void exitState (CharacterControllerStateBase state)
    {
        state.OnSetNewStateRequested -= setNewState;
        state.OnAddVelocityRequested -= addVelocity;
        state.OnSetVelocityRequested -= setVelocity;
        state.Exit();
    }

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        inputManager = BurinkeruInputManager.Instance;
        setNewState<InAirState>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPos = transform.position;

        updateMovement();
        applyForces();
        updateIsGrounded();
        checkForPushback();

        DeltaPosition = transform.position - startPos;
    }

    void updateMovement ()
    {
        Vector3 deltaPosition = Vector3.zero;
        Vector3 forwardDirection = this.transform.forward;
        Vector3 rightDirection = this.transform.right;
        float movementSpeed = getMovementSpeed();

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

        if (inputManager.IsCommandDown (BurinkeruInputManager.InputCommand.CROUCH))
        {
            switchCrouch();
        }

        deltaPosition.Normalize();
        deltaPosition *= movementSpeed;
        deltaPosition.Scale(movementAxes);

        if (IsGrounded)
        {
            move(deltaPosition * Time.deltaTime);
        }
        else
        {
            move(deltaPosition * Time.deltaTime / 10f);
            velocity += deltaPosition;
        }

        if (mainMovementState != null)
        {
            mainMovementState.UpdateMovement();
        }
    }

    void switchCrouch ()
    {
        if (crouchState == null)
        {
            EnterCrouch();
        }
        else
        {
            ExitCrouch();
        }
    }

    public void EnterCrouch ()
    {
        crouchState = new CrouchState();
        crouchState.Enter(null, inputManager, this, capsuleCollider);
    }

    public void ExitCrouch ()
    {
        if (crouchState != null)
        {
            crouchState.Exit();
            crouchState = null;
        }
    }

    float getMovementSpeed ()
    {
        float result = defaultMovementSpeed;

        if (mainMovementState != null)
        {
            result *= mainMovementState.GetMovementSpeedFactor();
        }

        if (crouchState != null && IsGrounded)
        {
            result *= crouchState.GetMovementSpeedFactor();
        }

        return result;
    }

    void applyForces ()
    {
        float drag = 1f - getMovementDrag();
        move(velocity * Time.deltaTime);
        velocity.Scale(new Vector3(drag, 1f, drag));

        if (mainMovementState != null)
        {
            mainMovementState.ApplyForces();
        }
    }

    void addVelocity (Vector3 velocityDelta)
    {
        velocity += velocityDelta;
    }

    void setVelocity (Vector3 newVelocty)
    {
        velocity = newVelocty; 
    }

    float getMovementDrag ()
    {
        if (IsGrounded)
        {
            return 0.1f;
        }
        else
        {
            return 0.005f;
        }
    }

    void updateIsGrounded ()
    {
        Vector3 sphereCastPos = transform.position - Vector3.up * capsuleCollider.height / 4f;
        float sphereCastRadius = capsuleCollider.radius;
        Collider[] colliders = Physics.OverlapSphere(sphereCastPos, sphereCastRadius);
        IsGrounded = false;

        for (int i = 0; i < colliders.Length; i ++)
        {
            if (colliders [i] != capsuleCollider)
            {
                Vector3 contactPoint = colliders[i].GetClosestPoint(sphereCastPos);
                Vector3 contactDirectionVector = contactPoint - sphereCastPos;
                Vector3 pushVector = sphereCastPos - contactPoint;
                transform.position += Vector3.ClampMagnitude(pushVector, Mathf.Clamp(sphereCastRadius - pushVector.magnitude, 0, sphereCastRadius));

                if (! (Mathf.Abs (contactDirectionVector.y) < 0.1f || Mathf.Abs (contactDirectionVector.x) > 0.4f || Math.Abs (contactDirectionVector.z) > 0.4)) //TODO magic numbers
                {
                    IsGrounded = true;
                }
            }
        }
    }

    void checkForPushback ()
    {
        Collider [] colliders = Physics.OverlapSphere(transform.position, capsuleCollider.radius);
        Vector3 contactPoint = Vector3.zero;

        for (int i = 0; i < colliders.Length; i++)
        {
            contactPoint = colliders[i].GetClosestPoint(transform.position);
            makePushback(contactPoint);
        }
    }

    void makePushback (Vector3 contactPoint)
    {
        Vector3 pushVector = transform.position - contactPoint;
        transform.position += Vector3.ClampMagnitude(pushVector, Mathf.Clamp(capsuleCollider.radius - pushVector.magnitude, 0, capsuleCollider.radius));
    }

    private void move(Vector3 deltaPosition)
    {
        this.transform.position += deltaPosition;
    }
}
