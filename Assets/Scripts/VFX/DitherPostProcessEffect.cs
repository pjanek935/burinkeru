using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DitherPostProcessEffect : CameraPostProcessEffect
{
    [SerializeField, Range(0.0f, 1.0f)] float ditherStrength = 0.1f;
    [SerializeField, Range(1, 32)] int colourDepth = 4;

    protected override void preRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            material.SetFloat("_DitherStrength", ditherStrength);
            material.SetInt("_ColourDepth", colourDepth);
        }
    }
}
