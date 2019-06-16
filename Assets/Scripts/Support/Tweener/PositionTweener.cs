using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTweener : TransformTweener
{
    public PositionTweener (GameObject gameObject, float duration, TweenerType tweenerType, 
        float targetValue, InterpolationType tweenerInterpolationType = InterpolationType.LINEAR, bool local = false,
        Tweener.TweenerEventHandler onFinished = null, float delay = 0f)
    {
        float startValue = 0;

        if (tweenerType == TweenerType.X || tweenerType == TweenerType.Y || tweenerType == TweenerType.Z)
        {
            switch (tweenerType)
            {
                case TweenerType.X:

                    if (local)
                    {
                        startValue = gameObject.transform.localPosition.x;
                    }
                    else
                    {
                        startValue = gameObject.transform.position.x;
                    }

                    break;

                case TweenerType.Y:

                    if (local)
                    {
                        startValue = gameObject.transform.localPosition.y;
                    }
                    else
                    {
                        startValue = gameObject.transform.position.y;
                    }

                    break;

                case TweenerType.Z:

                    if (local)
                    {
                        startValue = gameObject.transform.localPosition.z;
                    }
                    else
                    {
                        startValue = gameObject.transform.position.z;
                    }

                    break;
            }

            this.transform = gameObject.transform;
            this.local = local;
            this.setup (gameObject, duration, targetValue, startValue, tweenerType, tweenerInterpolationType);
            this.OnTweenerFinished += onFinished;
            this.delay = delay;
        }
        else
        {
            Cancel ();
        }
    }

    protected override void proceedAnimating()
    {
        base.proceedAnimating ();

        if (local)
		{
            Vector3 newPosition = transform.localPosition;
            newPosition.Scale (TransformTweener.GetInverseVectorForTweenerType (tweenerType));
            newPosition += currentValue * TransformTweener.GetVectorForTweenerType (tweenerType);

            transform.localPosition = newPosition;
        }
		else
		{
			Vector3 newPosition = transform.position;
            newPosition.Scale (TransformTweener.GetInverseVectorForTweenerType (tweenerType));
            newPosition += currentValue * TransformTweener.GetVectorForTweenerType (tweenerType);;

            transform.position = newPosition;
		}
    }
}
