using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenerComponent : MonoBehaviour
{
    List<Tweener> tweeners = new List<Tweener> ();

    public AnimationHandler AnimatePosition (Vector3 newPos, float duration, bool local = false, Tweener.InterpolationType interpolationType = Tweener.InterpolationType.LINEAR,
        Tweener.TweenerEventHandler onFinished = null, float delay = 0f)
    {
        AnimationHandler animationHandler = new AnimationHandler ();
        Vector3 currentPos = transform.position;

        if (local)
        {
            currentPos = transform.localPosition;
        }
        
        if (Mathf.Abs (currentPos.x - newPos.x) > Tweener.MinTargetDifference)
        {
            tweeners.Add (new PositionTweener (this.gameObject, duration, Tweener.TweenerType.X, newPos.x, interpolationType, local, onFinished, delay));
            animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
            onFinished = null;
        }

        if (Mathf.Abs (currentPos.y - newPos.y) > Tweener.MinTargetDifference)
        {
            tweeners.Add (new PositionTweener (this.gameObject, duration, Tweener.TweenerType.Y, newPos.y, interpolationType, local, onFinished, delay));
            animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
            onFinished = null;
        }

        if (Mathf.Abs (currentPos.z - newPos.z) > Tweener.MinTargetDifference)
        {
            tweeners.Add (new PositionTweener (this.gameObject, duration, Tweener.TweenerType.Z, newPos.z, interpolationType, local, onFinished, delay));
            animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
            onFinished = null;
        }

        return animationHandler;
    }

    public AnimationHandler AnimateScale (Vector3 newScale, float duration, Tweener.InterpolationType interpolationType = Tweener.InterpolationType.LINEAR)
    {
        AnimationHandler animationHandler = new AnimationHandler ();
        Vector3 currentScale = transform.localScale;

        if (Mathf.Abs (currentScale.x - newScale.x) > Tweener.MinTargetDifference)
        {
            tweeners.Add (new ScaleTweener (this.gameObject, duration, Tweener.TweenerType.X, newScale.x, interpolationType));
            animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
        }

        if (Mathf.Abs (currentScale.y - newScale.y) > Tweener.MinTargetDifference)
        {
            tweeners.Add (new ScaleTweener (this.gameObject, duration, Tweener.TweenerType.Y, newScale.y, interpolationType));
            animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
        }

        if (Mathf.Abs (currentScale.z - newScale.z) > Tweener.MinTargetDifference)
        {
            tweeners.Add (new ScaleTweener (this.gameObject, duration, Tweener.TweenerType.Z, newScale.z, interpolationType));
            animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
        }

        return animationHandler;
    }

    public AnimationHandler AnimateRotation (Vector3 newEulerRotation, float duration, 
        bool local = false, Tweener.InterpolationType interpolationType = Tweener.InterpolationType.LINEAR)
    {
        AnimationHandler animationHandler = new AnimationHandler ();
         Vector3 currentRotation = transform.eulerAngles;

        if (local)
        {
            currentRotation = transform.localEulerAngles;
        }
        
        if (Mathf.Abs (currentRotation.x - newEulerRotation.x) > Tweener.MinTargetDifference)
        {
            tweeners.Add (new RotationTweener (this.gameObject, duration, Tweener.TweenerType.X, newEulerRotation.x, interpolationType, local));
            animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
        }

        if (Mathf.Abs (currentRotation.y - newEulerRotation.y) > Tweener.MinTargetDifference)
        {
            tweeners.Add (new RotationTweener (this.gameObject, duration, Tweener.TweenerType.Y, newEulerRotation.y, interpolationType, local));
            animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
        }

        if (Mathf.Abs (currentRotation.z - newEulerRotation.z) > Tweener.MinTargetDifference)
        {
            tweeners.Add (new RotationTweener (this.gameObject, duration, Tweener.TweenerType.Z, newEulerRotation.z, interpolationType, local));
            animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
        }

        return animationHandler;
    }

    public AnimationHandler TweeenValue (float startValue, float endValue, float duration, Tweener.TweenerEventHandler onUpdate,
        Tweener.InterpolationType interpolationType = Tweener.InterpolationType.LINEAR, float delay = 0f)
    {
        AnimationHandler animationHandler = new AnimationHandler ();

        if (Mathf.Abs (startValue - endValue) > Tweener.MinTargetDifference)
        {
            tweeners.Add (new ValueTweener (this.gameObject, duration, startValue, onUpdate, endValue, interpolationType, delay));
            animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
        }

        return animationHandler;
    }

    public AnimationHandler AnimateCanvasGroupAlpha (float targetValue, float duration,
        Tweener.InterpolationType interpolationType = Tweener.InterpolationType.LINEAR, float delay = 0f, Tweener.TweenerEventHandler onFinished = null)
    {
        AnimationHandler animationHandler = new AnimationHandler ();
        CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup> ();
        targetValue = Mathf.Clamp01 (targetValue);

        if (canvasGroup != null)
        {
            if (Mathf.Abs (canvasGroup.alpha - targetValue) > Tweener.MinTargetDifference)
            {
                tweeners.Add (new CanvasAlphaTweener (this.gameObject, duration, targetValue, interpolationType, delay, onFinished));
                animationHandler.AddTweener (tweeners [tweeners.Count - 1]);
            }
        }

        return animationHandler;
    }

    private void Update()
    {
        if (tweeners != null)
        {
            for (int i = tweeners.Count - 1; i >= 0; i --)
            {
                tweeners [i].Update ();

                if (tweeners [i].Destroy)
                {
                    tweeners.RemoveAt (i);
                }
            }
        }
    }

    private void OnDisable()
    {
        CancelAll ();
    }

    public void CancelAll ()
    {
        if (tweeners != null)
        {
            for (int i = tweeners.Count - 1; i >= 0; i--)
            {
                tweeners [i].Cancel ();
            }
        }
    }
}
