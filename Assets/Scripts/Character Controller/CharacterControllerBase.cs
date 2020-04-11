using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerBase : MonoBehaviour
{
    protected CharacterStateBase mainMovementState;
    protected int layerMaskToCheckForPushback = 0;
    protected Vector3 externalDeltaPosition = Vector3.zero;
    protected bool preciseCollisionCalucations = true;
    public const int CollisionCalicationsPrecision = 20; //bigger the number more precise the caluclation

    public virtual CapsuleCollider CapsuleCollider
    {
        get { return null; }
    }

    public bool IsGrounded
    {
        get;
        protected set;
    }

    public Vector3 DeltaPosition
    {
        get;
        protected set;
    }

    public Vector3 Velocity
    {
        get;
        protected set;
    }

    protected void Update ()
    {
        Vector3 startPos = transform.position;

        updateMovement ();
        applyForces ();
        updatePosition ();
        updateIsGrounded ();
        checkForPushback ();

        DeltaPosition = transform.position - startPos;
    }

    public void SetNewState (CharacterStateBase newState)
    {
        if (mainMovementState == null || newState.GetType () != mainMovementState.GetType ())
        {
            if (mainMovementState != null)
            {
                exitState (mainMovementState);
            }

            enterState (newState);
            mainMovementState = newState;
        }
    }

    public virtual float GetMovementSpeed ()
    {
        return 1f;
    }

    public void Move (Vector3 deltaPosition)
    {
        externalDeltaPosition += deltaPosition;
    }

    public void AddVelocity (Vector3 velocityDelta)
    {
        Velocity += velocityDelta;
    }

    public void SetVelocity (Vector3 newVelocty)
    {
        Velocity = newVelocty;
    }

    public virtual float GetMovementDrag ()
    {
        float result = 1f;

        if (mainMovementState != null)
        {
            result = mainMovementState.GetMovementDrag ();
        }

        return result;
    }

    protected virtual void updateMovement ()
    {
        if (mainMovementState != null)
        {
            mainMovementState.UpdateMovement ();
        }
    }

    protected void updatePosition ()
    {
        if (preciseCollisionCalucations)
        {
            Vector3 d = externalDeltaPosition / (float) CollisionCalicationsPrecision;

            for (int i = 0; i < CollisionCalicationsPrecision; i++)
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

    protected void updateIsGrounded ()
    {
        if (CapsuleCollider == null)
        {
            return;
        }

        Vector3 sphereCastPos = transform.position - Vector3.up * CapsuleCollider.height / 4f + Vector3.up * CapsuleCollider.center.y;
        float sphereCastRadius = CapsuleCollider.radius;
        Collider [] colliders = Physics.OverlapSphere (sphereCastPos, sphereCastRadius, layerMaskToCheckForPushback);
        IsGrounded = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders [i] != CapsuleCollider)
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

    protected bool checkForPushback ()
    {
        bool result = false;
        
        if (CapsuleCollider == null)
        {
            return false;
        }

        Collider [] colliders = Physics.OverlapSphere (transform.position, CapsuleCollider.radius, layerMaskToCheckForPushback);
        Vector3 contactPoint = Vector3.zero;

        for (int i = 0; i < colliders.Length; i++)
        {
            result = true;
            contactPoint = colliders [i].GetClosestPoint (transform.position);
            makePushback (contactPoint);
        }

        return result;
    }

    protected void makePushback (Vector3 contactPoint)
    {
        if (CapsuleCollider == null)
        {
            return;
        }

        Vector3 pushVector = transform.position - contactPoint;
        transform.position += Vector3.ClampMagnitude (pushVector,
            Mathf.Clamp (CapsuleCollider.radius - pushVector.magnitude, 0, CapsuleCollider.radius));
    }

    protected void setNewState<T> () where T : CharacterStateBase
    {
        if (mainMovementState == null || typeof (T) != mainMovementState.GetType ())
        {
            CharacterStateBase newState = (T) Activator.CreateInstance (typeof (T));

            if (mainMovementState != null)
            {
                exitState (mainMovementState);
            }

            enterState (newState);
            mainMovementState = newState;
        }
    }

    protected virtual void enterState (CharacterStateBase state)
    {
        state.Enter (this);
    }

    protected void exitState (CharacterStateBase state)
    {
        state.Exit ();
    }

    protected virtual void applyForces ()
    {
        float friction = 1f - GetMovementDrag ();
        Vector3 velocitySum = Velocity;
        Move (velocitySum * Time.deltaTime);
        Vector3 v = Velocity;
        v.Scale (new Vector3 (friction, 1f, friction));
        Velocity = v;

        if (mainMovementState != null)
        {
            mainMovementState.ApplyForces ();
        }
    }
}
