using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CapsuleCollider))]
public class BurinkeruCharacterController : MonoBehaviour
{
    [SerializeField] CharacterComponents components = null;
    [SerializeField] CombatController combatController = null;
    [SerializeField] BlinkingController blinkingController;

    BurinkeruInputManager inputManager;
    CharacterControllerStateBase mainMovementState;
    CrouchState crouchState;

    public static Vector3 MovementAxes
    {
        get { return new Vector3(1f, 0f, 1f); }
    }

    public const float GRAVITY = 15f; //TODO does not suit in this class

    Vector3 velocity = Vector3.zero;

    public Vector3 DeltaPosition
    {
        get;
        private set;
    }

    public bool IsBlinking
    {
        get { return blinkingController.IsBlinking; }
    }

    public Vector3 Velocity
    {
        get { return velocity; }
    }

    public bool IsGrounded
    {
        get;
        private set;
    }

    public bool IsSliding ()
    {
        bool result = false;

        if (mainMovementState != null && mainMovementState is GroundState)
        {
            GroundState groundState = (GroundState)mainMovementState;
            result = groundState.IsSliding();
        }

        return result;
    }

    public bool IsCrouching
    {
        get { return crouchState != null; }
    }

    void setListenersToWeapon (WeaponBase weapon)
    {
        if (weapon != null)
        {
            weapon.OnAddVelocityRequested += addVelocity;
            weapon.OnSetVelocityRequested += setVelocity;
        }
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

    private void Awake()
    {
        combatController.OnSetListenersToWeaponRequested += setListenersToWeapon;
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
        state.OnMoveRequested += move;
        state.Enter(mainMovementState, inputManager, this, components);
    }

    void exitState (CharacterControllerStateBase state)
    {
        state.OnSetNewStateRequested -= setNewState;
        state.OnAddVelocityRequested -= addVelocity;
        state.OnSetVelocityRequested -= setVelocity;
        state.OnMoveRequested -= move;
        state.Exit();
    }

    // Start is called before the first frame update
    void Start()
    {
        inputManager = BurinkeruInputManager.Instance;
        setNewState<InAirState>();
        blinkingController.OnBlink += onBlink;
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
        blinkingController.UpdateBlinkingInput();

        if (mainMovementState != null)
        {
            mainMovementState.UpdateMovement();
        }

        if (crouchState != null)
        {
            crouchState.UpdateMovement();
        }
    }

    void onBlink ()
    {
        velocity = Vector3.zero;
    }

    public void EnterCrouch ()
    {
        crouchState = new CrouchState();
        crouchState.Enter(null, inputManager, this, components);
    }

    public void ExitCrouch ()
    {
        if (crouchState != null)
        {
            crouchState.Exit();
            crouchState = null;
        }
    }

    public Vector3 GetLookDirection ()
    {
        return components.FPPCamera.transform.forward;
    }

    public Vector3 GetUpwardDirection()
    {
        return components.FPPCamera.transform.up;
    }

    public float GetMovementSpeed ()
    {
        float result = CharacterControllerParameters.Instance.DefaultMoveSpeed;

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
        float friction = 1f - GetMovementDrag();
        Vector3 velocitySum = velocity;
        velocitySum += blinkingController.BlinkingVelocity;
        move(velocitySum * Time.deltaTime);
        velocity.Scale(new Vector3(friction, 1f, friction));
        blinkingController.UpdateBlinkingForces();

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

    public float GetMovementDrag ()
    {
        float result = 1f;

        if (mainMovementState != null)
        {
            result = mainMovementState.GetMovementDrag();
        }

        return result;
    }

    void updateIsGrounded ()
    {
        Vector3 sphereCastPos = transform.position - Vector3.up * components.CapsuleCollider.height / 4f;
        float sphereCastRadius = components.CapsuleCollider.radius;
        Collider[] colliders = Physics.OverlapSphere(sphereCastPos, sphereCastRadius);
        IsGrounded = false;

        for (int i = 0; i < colliders.Length; i ++)
        {
            if (colliders [i] != components.CapsuleCollider)
            {
                Vector3 contactPoint = colliders[i].GetClosestPoint(sphereCastPos);
                Vector3 contactDirectionVector = contactPoint - sphereCastPos;
                Vector3 pushVector = sphereCastPos - contactPoint;
                transform.position += Vector3.ClampMagnitude(pushVector, Mathf.Clamp(sphereCastRadius - pushVector.magnitude, 0, sphereCastRadius));

                if (! (Mathf.Abs (contactDirectionVector.y) < 0.1f || Mathf.Abs (contactDirectionVector.x) > 0.4f || Math.Abs (contactDirectionVector.z) > 0.4)) //TODO magic numbers
                {
                    IsGrounded = true;
                    blinkingController.ResetCounter();
                }
            }
        }
    }

    void checkForPushback ()
    {
        Collider [] colliders = Physics.OverlapSphere(transform.position, components.CapsuleCollider.radius);
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
        transform.position += Vector3.ClampMagnitude(pushVector, Mathf.Clamp(components.CapsuleCollider.radius - pushVector.magnitude, 0, components.CapsuleCollider.radius));
    }

    private void move(Vector3 deltaPosition)
    {
        this.transform.position += deltaPosition;
    } 
}
