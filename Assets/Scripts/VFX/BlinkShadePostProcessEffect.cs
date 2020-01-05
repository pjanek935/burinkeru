using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkShadePostProcessEffect : CameraPostProcessEffect
{
    float strength = 0;
    bool direction = true;
    [SerializeField] float maxStrength = 0.3f;
    [SerializeField]  float speed = 2f;

    public void StartEffect ()
    {
        this.enabled = true;
        direction = true;
    }

    public void StopEffect ()
    {
        direction = false;
    }

    private void Update()
    {
        if (direction)
        {
            strength -= Time.deltaTime * speed;

            if (strength < -maxStrength)
            {
                strength = -maxStrength;
            }
        }
        else
        {
            strength += Time.deltaTime * speed;

            if (strength > 0f)
            {
                strength = 0;
                this.enabled = false;
                direction = true;
            }
        }
    }

    protected override void preRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            material.SetFloat("_Strength", strength);
        }

        base.preRenderImage(source, destination);
    }
}
