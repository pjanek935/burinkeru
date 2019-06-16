using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAlphaTweener : Tweener
{
    CanvasGroup canvas;

    public CanvasAlphaTweener (GameObject gameObject, float duration,
        float targetValue, InterpolationType tweenerInterpolationType = InterpolationType.LINEAR, float delay = 0f, Tweener.TweenerEventHandler onFinished = null)
    {
        canvas = gameObject.GetComponent<CanvasGroup> ();

        if (canvas != null)
        {
            float startValue = canvas.alpha;
            setup (gameObject, duration, targetValue, startValue, TweenerType.CANVAS_ALPHA, interpolationType, delay);
            this.OnTweenerFinished += onFinished;
        }
        else
        {
            Cancel ();
        }
    }

    protected override void proceedAnimating()
    {
        base.proceedAnimating ();

        if (canvas != null)
        {
            canvas.alpha = currentValue;
        }
        else
        {
            Cancel ();
        }
    }
}
