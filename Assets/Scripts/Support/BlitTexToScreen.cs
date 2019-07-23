using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlitTexToScreen : MonoBehaviour
{
    [SerializeField] Material material = null;

    RenderTexture backTex;
    RenderTexture frontTex;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (backTex != null && frontTex != null && material != null)
        {
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    public void SetBackTexture (RenderTexture texture)
    {
        backTex = texture;
        material.SetTexture("_FrontTex", backTex);
    }

    public void SetFrontTexture (RenderTexture texture)
    {
        frontTex = texture;
        material.SetTexture("_BackTex", frontTex);
    }
}
