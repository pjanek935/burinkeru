using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ImageProcessing
{
    public static void SetAlpha (this Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }

	public static void Scale (this Color color, Color scale)
	{
		color.r *= scale.r;
		color.g *= scale.g;
		color.b *= scale.b;
		color.a *= scale.a;
	}

	public static Color Mul (float value, Color color) 
	{
		Color result = new Color ();
		result.r = color.r * value;
		result.g = color.g * value;
		result.b = color.b * value;
		result.a = color.a * value;

		return result;
	}		
}
