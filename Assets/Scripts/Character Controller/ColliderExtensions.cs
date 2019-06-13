using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderExtensions
{
    public static Vector3 GetClosestPoint (this SphereCollider sphereCollider, Vector3 to)
    {
        Vector3 result = Vector3.zero;
        result = to - sphereCollider.transform.position;
        result.Normalize();
        result *= sphereCollider.radius * sphereCollider.transform.localScale.x;
        result += sphereCollider.transform.position;

        return result;
    }

    public static Vector3 GetClosestPoint (this BoxCollider boxCollider, Vector3 to)
    {
        Vector3 result = Vector3.zero;
        Transform boxTransform = boxCollider.transform;
        Vector3 toInLocal = boxTransform.InverseTransformPoint(to);
        toInLocal -= boxCollider.center;
        toInLocal = new Vector3(Mathf.Clamp(toInLocal.x, -boxCollider.size.x * 0.5f, boxCollider.size.x * 0.5f),
            Mathf.Clamp(toInLocal.y, -boxCollider.size.y * 0.5f, boxCollider.size.y * 0.5f),
            Mathf.Clamp(toInLocal.z, -boxCollider.size.z * 0.5f, boxCollider.size.z * 0.5f));
        toInLocal += boxCollider.center;
        result = boxTransform.TransformPoint(toInLocal);

        return result;
    }

    public static Vector3 GetClosestPoint (this Collider collider, Vector3 to)
    {
        Vector3 result = Vector3.zero;

        if (collider is BoxCollider)
        {
            if (collider.transform.rotation == Quaternion.identity)
            {
                result = collider.ClosestPointOnBounds(to);
            }
            else
            {
                result = ((BoxCollider)collider).GetClosestPoint(to);
            }
        }
        else if (collider is SphereCollider)
        {
            result = ((SphereCollider)collider).GetClosestPoint(to);
        }
        else
        {
            result = collider.ClosestPointOnBounds(to);
        }

        return result;
    }
}
