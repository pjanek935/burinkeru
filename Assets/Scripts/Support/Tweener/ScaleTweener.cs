using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTweener : TransformTweener
{
    public ScaleTweener (GameObject gameObject, float duration, TweenerType tweenerType,
        float targetValue, InterpolationType tweenerInterpolationType = InterpolationType.LINEAR)
    {
        float startValue = 0;

        if (tweenerType == TweenerType.X || tweenerType == TweenerType.Y || tweenerType == TweenerType.Z)
        {
            switch (tweenerType)
            {
                case TweenerType.X:

                    startValue = gameObject.transform.localScale.x;

                    break;

                case TweenerType.Y:

                    startValue = gameObject.transform.localScale.y;

                    break;

                case TweenerType.Z:

                    startValue = gameObject.transform.localScale.z;

                    break;
            }

            this.transform = gameObject.transform;
            this.setup (gameObject, duration, targetValue, startValue, tweenerType, tweenerInterpolationType);
        }
        else
        {
            Cancel ();
        }
    }

    protected override void proceedAnimating()
    {
        base.proceedAnimating ();

        Vector3 newScale = transform.localScale;
        newScale.Scale (TransformTweener.GetInverseVectorForTweenerType (tweenerType));
        newScale += currentValue * TransformTweener.GetVectorForTweenerType (tweenerType);

        transform.localScale = newScale;
    }
}
