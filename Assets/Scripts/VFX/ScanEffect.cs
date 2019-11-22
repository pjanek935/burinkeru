using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanEffect : MonoBehaviour
{
    [SerializeField] ColorInversionPostProcessEffect distortion;
    [SerializeField] Transform scannerOrigin;
    [SerializeField] Material effectMaterial;
    [SerializeField] float maxDistance = 200f;
    [SerializeField] float forwardSpeed = 400f;
    [SerializeField] float backwardSpeed = 400f;

    float scanDistance;
    new Camera camera;
    bool direction = true;

    public bool IsScanning
    {
        get;
        private set;
    }

    void Start()
    {
      
    }

    public void StartEffect ()
    {
        IsScanning = true;
        direction = true;
        scanDistance = 0;
        distortion.strength = 0;
    }

    public void EndEffect ()
    {
        IsScanning = true;
        direction = false;

        if (scanDistance > maxDistance / 2f)
        {
            scanDistance = maxDistance / 2f;
        }

        distortion.strength = 1f;
    }

    void Update()
    {
        if (IsScanning)
        {
            if (direction)
            {
                scanDistance += Time.deltaTime * forwardSpeed;
                distortion.strength += Time.deltaTime * 6;

                if (distortion.strength > 1)
                {
                    distortion.strength = 1;
                }

                if (scanDistance > 400)
                {
                    IsScanning = false;
                }
            }
            else
            {
                scanDistance -= Time.deltaTime * backwardSpeed;

                if (scanDistance < 50)
                {
                    distortion.strength -= Time.deltaTime * 6;

                    if (distortion.strength < 0)
                    {
                        distortion.strength = 0;
                    }
                }

                if (scanDistance < 0)
                {
                    IsScanning = false;
                    scanDistance = 0;
                    distortion.strength = 0;
                }

            }
        }
    }
    // End Demo Code

    void OnEnable()
    {
        camera = GetComponent<Camera>();
        camera.depthTextureMode = DepthTextureMode.Depth;
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        effectMaterial.SetVector("_WorldSpaceScannerPos", scannerOrigin.position);
        effectMaterial.SetFloat("_ScanDistance", scanDistance);
        RaycastCornerBlit(src, dst, effectMaterial);
    }

    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
    {
        // Compute Frustum Corners
        float camFar = camera.farClipPlane;
        float camFov = camera.fieldOfView;
        float camAspect = camera.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (camera.transform.forward - toRight + toTop);
        float camScale = topLeft.magnitude * camFar;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (camera.transform.forward + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (camera.transform.forward + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (camera.transform.forward - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= camScale;

        // Custom Blit, encoding Frustum Corners as additional Texture Coordinates
        RenderTexture.active = dest;

        mat.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.MultiTexCoord(1, bottomLeft);
        GL.Vertex3(0.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.MultiTexCoord(1, bottomRight);
        GL.Vertex3(1.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.MultiTexCoord(1, topRight);
        GL.Vertex3(1.0f, 1.0f, 0.0f);

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.MultiTexCoord(1, topLeft);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
    }
}
