using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tweener
{
    public enum TweenerType
    {
        X, Y, Z, ALPHA, R, G, B, A, VALUE, CANVAS_ALPHA,
    }
    
    public enum InterpolationType
    {
        LINEAR, EASE_IN, EASE_OUT, EASE_IN_OUT
    }

    public const float MinTargetDifference = 0.0001f;

    public delegate void TweenerEventHandler(Tweener tweener, float currentValue);
    public event TweenerEventHandler OnValueUpdate;
    public event TweenerEventHandler OnTweenerPaused;
    public event TweenerEventHandler OnTweenerResumed;
    public event TweenerEventHandler OnTweenerCanceled;
    public event TweenerEventHandler OnTweenerFinished;

    protected bool animating = false;
    protected bool destroy = false;
    bool animationEnded = false;
    bool endInterpolation = false;
    protected float progress = 0f;
    protected TweenerType tweenerType;
    protected InterpolationType interpolationType;
    protected GameObject gameObject;

    protected float delay = 0f;
    protected float duration = 0f;
    protected float targetValue;
    protected float startValue;
    protected float currentValue;
   

    public bool Animating
    {
        get { return animating; }
    }

    public bool Destroy
    {
        get { return destroy; }
    }

    public TweenerType Type
    {
        get { return tweenerType; }
    }

    public InterpolationType Interpolation
    {
        get { return interpolationType; }
    }

    public void Pause ()
    {
        if (! destroy && animating)
        {
            animating = false;

            if (OnTweenerPaused != null)
            {
                OnTweenerPaused (this, currentValue);
            }
        }
    }

    public void Cancel ()
    {
        animating = false;
        destroy = true;

        if (OnTweenerCanceled != null)
        {
            OnTweenerCanceled (this, currentValue);
        }
    }

    public void Resume ()
    {
        if (! destroy && ! animating)
        {
            animating = true;

            if (OnTweenerResumed != null)
            {
                OnTweenerResumed (this, currentValue);
            }
        }
    }

    protected void setup (GameObject gameObject, float duration, float targetValue, float startValue, TweenerType tweenerType, 
        InterpolationType tweenerInterpolationType = InterpolationType.LINEAR, float delay = 0f)
    {
        if (delay > 0)
        {
            this.delay = delay;
        }
        
        this.targetValue = targetValue;
        this.gameObject = gameObject;
        this.tweenerType = tweenerType;
        this.duration = duration;
        this.interpolationType = tweenerInterpolationType;
        this.startValue = startValue;
        animating = true;
    }

    public void Update()
    {
        if (! destroy)
        {
            if (animating)
            {
                proceedAnimating ();
            }
        }
    }

    protected virtual void proceedAnimating ()
    {
        if (delay > 0)
        {
            progress += Time.deltaTime;

            if (progress >= delay)
            {
                delay = 0f;
                progress = 0f;
            }
        }
        else
        {
            if (endInterpolation)
            {
                progress = duration;
            }
            else
            {
                progress += Time.deltaTime;
            }

            if (progress + Time.deltaTime >= duration)
            {
                endInterpolation = true;
            }

            currentValue = getCurrentValue ();

            checkEndConditions ();

            if (animationEnded)
            {
                currentValue = targetValue;
            }

            if (OnValueUpdate != null)
            {
                OnValueUpdate (this, currentValue);
            }

            if (animationEnded)
            {
                if (OnTweenerFinished != null)
                {
                    OnTweenerFinished (this, currentValue);
                }
            }
        }
    }

    public void Finish ()
    {
        destroy = true;
        animationEnded = true;
        animating = false;
        endInterpolation = true;
        proceedAnimating ();
    }

    protected virtual void checkEndConditions ()
    {
        if (progress >= duration)
        {
            animationEnded = true;
            Cancel ();
        }
        else if (Mathf.Abs (targetValue - currentValue) <= MinTargetDifference)
        {
            animationEnded = true;
            Cancel ();
        }
    }

    float getCurrentValue ()
    {
        float result = 0;
        result = Interpolate (this.startValue, this.targetValue, this.progress, this.duration, this.interpolationType);

        return result;
    }

    public static float Interpolate (float startValue, float targetValue, float progress, float duration, InterpolationType interpolationType)
    {
        float result = 0;

        switch (interpolationType)
        {
            case InterpolationType.LINEAR:

                result = interpolateLinear (startValue, targetValue, progress, duration);

                break;

                case InterpolationType.EASE_IN:

                result = interpolateEaseIn (startValue, targetValue, progress, duration);

                break;

                case InterpolationType.EASE_OUT:

                result = interpolateEaseOut (startValue, targetValue, progress, duration);

                break;

                case InterpolationType.EASE_IN_OUT:

                result = interpolateEaseInEaseOut (startValue, targetValue, progress, duration);

                break;
        }

        return result;
    }

    static float interpolateLinear (float startValue, float targetValue, float progress, float duration)
    {
        float result = 0;
        result = startValue + (targetValue - startValue) * (progress / duration);

        return result;
    }

    static float interpolateEaseIn (float startValue, float targetValue, float progress, float duration)
    {
        float result = 0;
        float t = (progress / duration);
        t = t * t;
        result = startValue + (targetValue - startValue) * t;

        return result;
    }

    static float interpolateEaseOut (float startValue, float targetValue, float progress, float duration)
    {
        float result = 0;
        float t = (progress / duration);
        t = 1 - ((1-t)*(1-t));
        result = startValue + (targetValue - startValue) * t;

        return result;
    }

    static float interpolateEaseInEaseOut (float startValue, float targetValue, float progress, float duration)
    {
        float result = 0;
        float t = (progress / duration);
        t = (3 * t * t) - (2 * t * t * t);
        result = startValue + (targetValue - startValue) * t;

        return result;
    }
}
