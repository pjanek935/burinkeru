using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class NPCController : CharacterControllerBase
{
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] Hittable hittable;
    [SerializeField] Animator animator;
    [SerializeField] Transform playerTransform;

    public Transform PlayerTransform
    {
        get { return playerTransform; }
    }

    public Animator Animator
    {
        get { return animator; }
    }

    public override CapsuleCollider CapsuleCollider
    {
        get { return capsuleCollider; }
    }

    private void Awake ()
    {
        layerMaskToCheckForPushback = LayerMask.GetMask ("Default");
        setNewState<NPCInAirState> ();
        hittable.OnHitterActivated += onHitterActivated;
        hittable.OnHitterEnter += onHitterEnter;
    }

    void onHitterEnter (Hitter hitter)
    {
        if (hitter.HitterType == HitterType.PROJECTILE)
        {
            if (! IsGrounded)
            {
                Vector3 velocity = this.Velocity;
                velocity.y = 5.5f;
                SetVelocity (velocity);
            }

            onHit (hitter);
        }
    }

    void onHitterActivated (ActivatableHitter hitter, Hashtable parameters)
    {
        if (! IsGrounded)
        {
            Vector3 velocity = this.Velocity;
            velocity.y = 5.5f;
            SetVelocity (velocity);
        }

        if (hitter != null && hitter.HitterType == HitterType.BLADE)
        {
            if (parameters != null && parameters.ContainsKey (ParameterType.ATTACK_TYPE))
            {
                WeaponActionType attackType = (WeaponActionType) parameters [ParameterType.ATTACK_TYPE];
                Hashtable feedbackParameters = new Hashtable ();
                feedbackParameters.Add (ParameterType.SENDER_TYPE, SenderType.NPC);
                
                if (mainMovementState != null &&
                    mainMovementState is NPCGroundState &&
                    Random.value < 0.5f)
                {
                    attackType = WeaponActionType.BLOCK;
                    feedbackParameters.Add (ParameterType.ATTACK_TYPE, WeaponActionType.BLOCK);
                }

                hitter.SendMessage (feedbackParameters);

                switch (attackType)
                {
                    case WeaponActionType.SLASH:

                        onHit (hitter);

                        break;

                    case WeaponActionType.UPPERCUT:

                        upperCut ();

                        break;

                    case WeaponActionType.STAB:

                        stab ();

                        break;

                    case WeaponActionType.BLOCK:

                        block ();

                        break;
                }
            }
        }
    }

    void block ()
    {
        if (mainMovementState != null && mainMovementState is NPCGroundState)
        {
            NPCState npcState = (NPCState) mainMovementState;
            npcState.Block ();
        }
    }

    void onHit (Hitter hitter)
    {
        if (mainMovementState != null && mainMovementState is NPCState)
        {
            NPCState npcState = (NPCState) mainMovementState;
            npcState.OnHit (hitter);
        }
    }

    protected new void Update ()
    {
        base.Update ();
    }

    void upperCut ()
    {
        if (mainMovementState != null && mainMovementState is NPCState)
        {
            NPCState npcState = (NPCState) mainMovementState;
            npcState.Uppercut ();
        }
    }

    void stab ()
    {
        if (mainMovementState != null && mainMovementState is NPCState)
        {
            NPCState npcState = (NPCState) mainMovementState;
            npcState.Stab ();
        }
    }

    public override float GetMovementDrag ()
    {
        float result = 0.02f;

        if (mainMovementState != null)
        {
            result = mainMovementState.GetMovementDrag ();
        }

        return result;
    }
}
