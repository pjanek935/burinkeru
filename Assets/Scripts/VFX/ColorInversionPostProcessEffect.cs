using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorInversionPostProcessEffect : CameraPostProcessEffect
{
    [SerializeField, Range (0f, 1f)] public float strength = 0.5f;

    protected override void preRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            material.SetFloat("_Strength", strength);
        }
    }
}
