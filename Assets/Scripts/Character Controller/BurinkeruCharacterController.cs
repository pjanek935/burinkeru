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
    int layerMaskToCheckForPushback = 0;
    Vector3 externalDeltaPosition = Vector3.zero;
    SpecialAbility specialAbility = new BulletTime();
    bool preciseCollisionCalucations = true;
    const int collisionCalicationsPrecision = 20; //bigger the number more precise the caluclation

    public static Vector3 MovementAxes
    {
        get { return new Vector3(1f, 0f, 1f); }
    }

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
            weapon.OnAddVelocityRequested += AddVelocity;
            weapon.OnSetVelocityRequested += SetVelocity;
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
        layerMaskToCheckForPushback = LayerMask.GetMask("Default");
    }

    public void SetNewState (CharacterControllerStateBase newState)
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
        state.Enter(mainMovementState, inputManager, this, components);
    }

    void exitState (CharacterControllerStateBase state)
    {
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
        updatePosition();
        updateIsGrounded();
        checkForPushback();

        DeltaPosition = transform.position - startPos;

        if (Input.GetKeyDown (KeyCode.F))
        {
            if (specialAbility.IsActive)
            {
                specialAbility.Exit();
            }
            else
            {
                specialAbility.Enter(components);
            }
        }
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

    void updatePosition ()
    {
        if (preciseCollisionCalucations)
        {
            Vector3 d = externalDeltaPosition / (float) collisionCalicationsPrecision;

            for (int i = 0; i < collisionCalicationsPrecision; i ++)
            {
                transform.position += d;
                checkForPushback ();
            }

            externalDeltaPosition = Vector3.zero;
        }
        else
        {
            transform.position += externalDeltaPosition;
            externalDeltaPosition = Vector3.zero;
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
        Move(velocitySum * Time.deltaTime);
        velocity.Scale(new Vector3(friction, 1f, friction));
        blinkingController.UpdateBlinkingForces();

        if (mainMovementState != null)
        {
            mainMovementState.ApplyForces();
        }
    }

    public void AddVelocity (Vector3 velocityDelta)
    {
        velocity += velocityDelta;
    }

    public void SetVelocity (Vector3 newVelocty)
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
        Collider[] colliders = Physics.OverlapSphere(sphereCastPos, sphereCastRadius, layerMaskToCheckForPushback);
        IsGrounded = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != components.CapsuleCollider)
            {
                Vector3 contactPoint = colliders[i].GetClosestPoint(sphereCastPos);
                Vector3 contactDirectionVector = contactPoint - sphereCastPos;
                Vector3 pushVector = sphereCastPos - contactPoint;
                transform.position += Vector3.ClampMagnitude(pushVector, Mathf.Clamp(sphereCastRadius - pushVector.magnitude, 0, sphereCastRadius));

                if (!(Mathf.Abs(contactDirectionVector.y) < 0.1f
                    || Mathf.Abs(contactDirectionVector.x) > 0.4f
                    || Math.Abs(contactDirectionVector.z) > 0.4)) //TODO magic numbers
                {
                    IsGrounded = true;
                    blinkingController.ResetCounter();
                }
            }
        }
    }

    bool checkForPushback ()
    {
        bool result = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, components.CapsuleCollider.radius, layerMaskToCheckForPushback);
        Vector3 contactPoint = Vector3.zero;

        for (int i = 0; i < colliders.Length; i++)
        {
            result = true;
            contactPoint = colliders[i].GetClosestPoint(transform.position);
            makePushback(contactPoint);

            if (IsBlinking)
            {
                velocity = Vector3.Scale(velocity, new Vector3(0f, 1f, 0f));
                blinkingController.ForceStop();
            }
        }

        return result;
    }

    void makePushback (Vector3 contactPoint)
    {
        Vector3 pushVector = transform.position - contactPoint;
        transform.position += Vector3.ClampMagnitude(pushVector,
            Mathf.Clamp(components.CapsuleCollider.radius - pushVector.magnitude, 0, components.CapsuleCollider.radius));
    }

    public void Move(Vector3 deltaPosition)
    {
        externalDeltaPosition += deltaPosition;
    } 
}
