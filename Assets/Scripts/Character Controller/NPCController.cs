using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (parameters != null && parameters.ContainsKey ("attackIndex"))
            {
                int attackIndex = (int) parameters ["attackIndex"];

                Debug.Log ("Attack Index: " + attackIndex);

                if (attackIndex == 3)
                {
                    upperCut ();
                }
                else if (attackIndex == 4)
                {
                    stab ();
                }
            }
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
