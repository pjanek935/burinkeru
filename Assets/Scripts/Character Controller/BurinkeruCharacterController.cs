using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CapsuleCollider))]
public class BurinkeruCharacterController : CharacterControllerBase
{
    [SerializeField] CharacterComponents components = null;
    [SerializeField] CombatController combatController = null;
    [SerializeField] BlinkingController blinkingController;

    BurinkeruInputManager inputManager;
    PlayerCrouchState crouchState;
    SpecialAbility specialAbility = new BulletTime ();

    public static Vector3 MovementAxes
    {
        get { return new Vector3 (1f, 0f, 1f); }
    }

    public bool IsBlinking
    {
        get { return blinkingController.IsBlinking; }
    }

    public override CapsuleCollider CapsuleCollider
    {
        get { return components.CapsuleCollider; }
    }

    public bool IsSliding ()
    {
        bool result = false;

        if (mainMovementState != null && mainMovementState is PlayerGroundState)
        {
            PlayerGroundState groundState = (PlayerGroundState) mainMovementState;
            result = groundState.IsSliding ();
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

    private void Awake ()
    {
        combatController.OnSetListenersToWeaponRequested += setListenersToWeapon;
        layerMaskToCheckForPushback = LayerMask.GetMask ("Default");
    }

    protected override void enterState (CharacterStateBase state)
    {
        if (state is PlayerState)
        {
            PlayerState playerCharacterControllerStateBase = (PlayerState) state;
            playerCharacterControllerStateBase.Enter (inputManager, this, components);

            if (state is PlayerGroundState)
            {
                blinkingController.ResetCounter ();
            }
        }
    }

    // Start is called before the first frame update
    void Start ()
    {
        inputManager = BurinkeruInputManager.Instance;
        setNewState<PlayerInAirState> ();
        blinkingController.OnBlink += onBlink;
    }

    // Update is called once per frame
    protected new void Update ()
    {
        base.Update ();

        if (Input.GetKeyDown (KeyCode.F))
        {
            if (specialAbility.IsActive)
            {
                specialAbility.Exit ();
            }
            else
            {
                specialAbility.Enter (components);
            }
        }
        else if (Input.GetKeyDown (KeyCode.P))
        {
            components.RigManager.CurrentRig.SetTrigger ("Wave");
        }
    }

    override protected void updateMovement ()
    {
        blinkingController.UpdateBlinkingInput ();
        base.updateMovement ();

        if (crouchState != null)
        {
            crouchState.UpdateMovement ();
        }
    }

    void onBlink ()
    {
        Velocity = Vector3.zero;

        if (mainMovementState is PlayerGroundState)
        {
            blinkingController.ResetCounter ();
        }
    }

    public void EnterCrouch ()
    {
        crouchState = new PlayerCrouchState ();
        crouchState.Enter (inputManager, this, components);
    }

    public void ExitCrouch ()
    {
        if (crouchState != null)
        {
            crouchState.Exit ();
            crouchState = null;
        }
    }

    public Vector3 GetLookDirection ()
    {
        return components.FPPCamera.transform.forward;
    }

    public Vector3 GetUpwardDirection ()
    {
        return components.FPPCamera.transform.up;
    }

    public override float GetMovementSpeed ()
    {
        float result = CharacterControllerParameters.Instance.DefaultMoveSpeed;

        if (mainMovementState != null)
        {
            result *= mainMovementState.GetMovementSpeedFactor ();
        }

        if (crouchState != null && IsGrounded)
        {
            result *= crouchState.GetMovementSpeedFactor ();
        }

        return result;
    }

    override protected void applyForces ()
    {
        float friction = 1f - GetMovementDrag ();
        Vector3 velocitySum = Velocity;
        velocitySum += blinkingController.BlinkingVelocity;
        Move (velocitySum * Time.deltaTime);
        Vector3 v = Velocity;
        v.Scale (new Vector3(friction, 1f, friction));
        Velocity = v;
        blinkingController.UpdateBlinkingForces ();

        if (mainMovementState != null)
        {
            mainMovementState.ApplyForces ();
        }
    }
}
