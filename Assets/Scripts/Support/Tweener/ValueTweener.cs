using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueTweener : Tweener
{
    public ValueTweener (GameObject gameObject, float duration, float startValue, Tweener.TweenerEventHandler onUpdate,
            float targetValue, InterpolationType tweenerInterpolationType = InterpolationType.LINEAR, float delay = 0f)
    {
        setup (gameObject, duration, targetValue, startValue, TweenerType.VALUE, interpolationType, delay);
        this.OnValueUpdate += onUpdate;
    }
}
