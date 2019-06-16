using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransformTweener : Tweener 
{
    protected bool local = false;
    protected Transform transform;

    public static Vector3 GetVectorForTweenerType (TweenerType tweenerType)
    {
        Vector3 result = Vector3.one;

        if (tweenerType == TweenerType.X || tweenerType == TweenerType.Y || tweenerType == TweenerType.Z)
        {
            switch (tweenerType)
            {
                case TweenerType.X:

                result = new Vector3 (1, 0, 0);

                break;

                case TweenerType.Y:

                result = new Vector3 (0, 1, 0);

                break;

                case TweenerType.Z:

                result = new Vector3 (0, 0, 1);

                break;
            }
        }

        return result;
    }

    public static Vector3 GetInverseVectorForTweenerType (TweenerType tweenerType)
    {
        Vector3 result = Vector3.one;

        if (tweenerType == TweenerType.X || tweenerType == TweenerType.Y || tweenerType == TweenerType.Z)
        {
            switch (tweenerType)
            {
                case TweenerType.X:

                result = new Vector3 (0, 1, 1);

                break;

                case TweenerType.Y:

                result = new Vector3 (1, 0, 1);

                break;

                case TweenerType.Z:

                result = new Vector3 (1, 1, 0);

                break;
            }
        }

        return result;
    }

    public static float GetValueForTweenerType (Vector3 vector, TweenerType tweenerType)
    {
        float result = 0;

        if (tweenerType == TweenerType.X || tweenerType == TweenerType.Y || tweenerType == TweenerType.Z)
        {
            switch (tweenerType)
            {
                case TweenerType.X:

                result = vector.x;

                break;

                case TweenerType.Y:

                result = vector.y;

                break;

                case TweenerType.Z:

                result = vector.z;

                break;
            }
        }

        return result;
    }
}
