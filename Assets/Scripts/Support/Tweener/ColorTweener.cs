using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTweener : Tweener
{
	Graphic graphic;
	SpriteRenderer spriteRenderer;

	public ColorTweener (GameObject gameObject, float duration, TweenerType tweenerType, 
        	float targetValue, InterpolationType tweenerInterpolationType = InterpolationType.LINEAR, bool local = false)
	{
		graphic = gameObject.GetComponent <Graphic> ();

		if (graphic == null)
		{
			spriteRenderer = gameObject.GetComponent <SpriteRenderer> ();
		}

		if (graphic == null && spriteRenderer == null)
		{
			Cancel ();
		}
		else if (tweenerType == TweenerType.R || tweenerType == TweenerType.G || tweenerType == TweenerType.B || tweenerType == TweenerType.A)
		{
			targetValue = Mathf.Clamp (targetValue, 0f, 1f);
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

		Color newColor = new Color (0, 0, 0, 0);

		if (graphic != null)
		{
			newColor = graphic.color;
		}
		else if (spriteRenderer != null)
		{
			newColor = spriteRenderer.color;
		}

		newColor.Scale (ColorTweener.GetInverseColorForTweenerType (tweenerType));
        newColor += ImageProcessing.Mul (currentValue, ColorTweener.GetColorForTweenerType (tweenerType));

        if (graphic != null)
		{
			graphic.color = newColor;
		}
		else if (spriteRenderer != null)
		{
			spriteRenderer.color = newColor;
		}
    }

	public static Color GetInverseColorForTweenerType (TweenerType tweenerType)
    {
        Color result = new Color (0, 0, 0, 0);

        if (tweenerType == TweenerType.R || tweenerType == TweenerType.G || tweenerType == TweenerType.B || tweenerType == TweenerType.A)
        {
            switch (tweenerType)
            {
                case TweenerType.R:

                result = new Color (0, 1, 1, 1);

                break;

                case TweenerType.G:

                result = new Color (1, 0, 1, 1);

                break;

                case TweenerType.B:

                result = new Color (1, 1, 0, 1);

                break;

				case TweenerType.A:

                result = new Color (1, 1, 1, 0);

                break; 
            }
        }

        return result;
    }

    public static float GetValueForTweenerType (Color color, TweenerType tweenerType)
    {
        float result = 0;

        if (tweenerType == TweenerType.R || tweenerType == TweenerType.G || tweenerType == TweenerType.B || tweenerType == TweenerType.A)
        {
            switch (tweenerType)
            {
                case TweenerType.R:

                result = color.r;

                break;

                case TweenerType.G:

                result = color.g;

                break;

                case TweenerType.B:

                result = color.b;

                break;

				case TweenerType.A:

                result = color.a;

                break;
            }
        }

        return result;
    }

	public static Color GetColorForTweenerType (TweenerType tweenerType)
    {
        Color result = new Color (0, 0, 0, 0);

        if (tweenerType == TweenerType.R || tweenerType == TweenerType.G || tweenerType == TweenerType.B || tweenerType == TweenerType.A)
        {
            switch (tweenerType)
            {
                case TweenerType.R:

                result = new Color (1, 0, 0, 0);

                break;

                case TweenerType.G:

                result = new Color (0, 1, 0, 0);

                break;

                case TweenerType.B:

                result = new Color (0, 0, 1, 0);

                break;

				case TweenerType.A:

                result = new Color (0, 0, 0, 1);

                break;
            }
        }

        return result;
    }
}
