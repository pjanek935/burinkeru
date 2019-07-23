using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class downres : MonoBehaviour
{
    public int downscaleFactor = 0;
    public BlitTexToScreen mainCam;
    private Camera cam;
    private RenderTexture tex;
    private int oldScreenWidth;
    private int oldScreenHeight;
    private int oldDownscaleFactor;

    public bool frontTexture = true;

    void setTargetTexture()
    {
        // >> is a bitwise shift. In this context it basically just means divide screen width by 2^n, so we step down by factors of two
        if (cam.targetTexture != null)
        {
            cam.targetTexture.Release();
        }
        tex = new RenderTexture(Screen.width >> downscaleFactor, Screen.height >> downscaleFactor, 24, RenderTextureFormat.Default);
        tex.filterMode = FilterMode.Point;
        cam.targetTexture = tex;
       
        if (frontTexture)
        {
            mainCam.SetFrontTexture(tex);
        }
        else
        {
            mainCam.SetBackTexture(tex);
        }
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        oldScreenHeight = Screen.height;
        oldScreenWidth = Screen.width;
        oldDownscaleFactor = downscaleFactor;
        setTargetTexture();
    }

    void Update()
    {
        if (Screen.width != oldScreenWidth || Screen.height != oldScreenHeight || oldDownscaleFactor != downscaleFactor)
        {
            oldScreenWidth = Screen.width;
            oldScreenHeight = Screen.height;
            oldDownscaleFactor = downscaleFactor;
            setTargetTexture();
        }
    }

}
