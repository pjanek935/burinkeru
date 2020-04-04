using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] CapsuleCollider capsuleCollider;
    CharacterControllerStateBase mainMovementState;
    Vector3 velocity = Vector3.zero;
    int layerMaskToCheckForPushback = 0;
    Vector3 externalDeltaPosition = Vector3.zero;

    public bool IsGrounded
    {
        get;
        private set;
    }

    void applyForces ()
    {
        float friction = 1f - GetMovementDrag ();
        Vector3 velocitySum = velocity;
        Move (velocitySum * Time.deltaTime);
        velocity.Scale (new Vector3 (friction, 1f, friction));

        if (mainMovementState != null)
        {
            mainMovementState.ApplyForces ();
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
            result = mainMovementState.GetMovementDrag ();
        }

        return result;
    }

    void updateIsGrounded ()
    {
        Vector3 sphereCastPos = transform.position - Vector3.up * capsuleCollider.height / 4f;
        float sphereCastRadius = capsuleCollider.radius;
        Collider [] colliders = Physics.OverlapSphere (sphereCastPos, sphereCastRadius, layerMaskToCheckForPushback);
        IsGrounded = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders [i] != capsuleCollider)
            {
                Vector3 contactPoint = colliders [i].GetClosestPoint (sphereCastPos);
                Vector3 contactDirectionVector = contactPoint - sphereCastPos;
                Vector3 pushVector = sphereCastPos - contactPoint;
                transform.position += Vector3.ClampMagnitude (pushVector, Mathf.Clamp (sphereCastRadius - pushVector.magnitude, 0, sphereCastRadius));

                if (!(Mathf.Abs (contactDirectionVector.y) < 0.1f
                    || Mathf.Abs (contactDirectionVector.x) > 0.4f
                    || Mathf.Abs (contactDirectionVector.z) > 0.4)) //TODO magic numbers
                {
                    IsGrounded = true;
                }
            }
        }
    }

    bool checkForPushback ()
    {
        bool result = false;
        Collider [] colliders = Physics.OverlapSphere (transform.position, capsuleCollider.radius, layerMaskToCheckForPushback);
        Vector3 contactPoint = Vector3.zero;

        for (int i = 0; i < colliders.Length; i++)
        {
            result = true;
            contactPoint = colliders [i].GetClosestPoint (transform.position);
            makePushback (contactPoint);
        }

        return result;
    }

    void makePushback (Vector3 contactPoint)
    {
        Vector3 pushVector = transform.position - contactPoint;
        transform.position += Vector3.ClampMagnitude (pushVector,
            Mathf.Clamp (capsuleCollider.radius - pushVector.magnitude, 0, capsuleCollider.radius));
    }

    public void Move (Vector3 deltaPosition)
    {
        externalDeltaPosition += deltaPosition;
    }
}
