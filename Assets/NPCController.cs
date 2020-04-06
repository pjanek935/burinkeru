using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : CharacterControllerBase
{
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] Hittable hittable;
    [SerializeField] Animator animator;

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
            }
        }
    }

    protected new void Update ()
    {
        base.Update ();
        animator.ResetTrigger ("GetUp");

        if (mainMovementState is NPCInAirState)
        {
            if (Velocity.y < 0)
            {
                animator.SetTrigger ("FallDown");
            }
        }
        else if (mainMovementState is NPCGroundState)
        {
            if (Mathf.Abs (Velocity.y) < float.Epsilon)
            {
                animator.SetTrigger ("GetUp");
            }
        }
    }

    void upperCut ()
    {
        Vector3 velocity = this.Velocity;
        velocity.y = 14f;
        SetVelocity (velocity);
        animator.SetTrigger ("Uppercut");
    }
}
