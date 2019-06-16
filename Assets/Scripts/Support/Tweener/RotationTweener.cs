using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTweener : TransformTweener 
{
	public RotationTweener (GameObject gameObject, float duration, TweenerType tweenerType, 
        float targetValue, InterpolationType tweenerInterpolationType = InterpolationType.LINEAR, bool local = false)
	{
		this.local = local;
		this.transform = gameObject.transform;
		Vector3 rotation = transform.eulerAngles;

		if (local)
		{
			rotation = transform.localEulerAngles;
		}

		float startValue = TransformTweener.GetValueForTweenerType (rotation, tweenerType);
		setup (gameObject, duration, targetValue, startValue, tweenerType, tweenerInterpolationType);
	}

	protected override void proceedAnimating()
	{
		base.proceedAnimating ();

		if (local)
		{
			Vector3 newRotation = transform.localEulerAngles;
            newRotation.Scale (TransformTweener.GetInverseVectorForTweenerType (tweenerType));
            newRotation += currentValue * TransformTweener.GetVectorForTweenerType (tweenerType);;

            transform.localEulerAngles = newRotation;
		}
		else
		{
			Vector3 newRotation = transform.eulerAngles;
            newRotation.Scale (TransformTweener.GetInverseVectorForTweenerType (tweenerType));
            newRotation += currentValue * TransformTweener.GetVectorForTweenerType (tweenerType);;

            transform.eulerAngles = newRotation;
		}
	}
}
