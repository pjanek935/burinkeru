using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanEffect : MonoBehaviour
{
    [SerializeField] DistortionPostProcessEffect distortion;
    public Transform ScannerOrigin;
    public Material EffectMaterial;
    public float ScanDistance;

    private Camera _camera;
    bool direction = true;

    // Demo Code
    bool _scanning;

    void Start()
    {
      
    }

    public void StartEffect ()
    {
        _scanning = true;
        direction = true;
        ScanDistance = 0;
        distortion.strength = 0;
    }

    public void EndEffect ()
    {
        _scanning = true;
        direction = false;
        ScanDistance = 400;
        distortion.strength = 1f;
    }

    void Update()
    {
        if (_scanning)
        {
            if (direction)
            {
                ScanDistance += Time.deltaTime * 200;
                distortion.strength += Time.deltaTime * 6;

                if (distortion.strength > 1)
                {
                    distortion.strength = 1;
                }

                if (ScanDistance > 400)
                {
                    _scanning = false;
                }
            }
            else
            {
                ScanDistance -= Time.deltaTime * 400;

                if (ScanDistance < 50)
                {
                    distortion.strength -= Time.deltaTime * 6;

                    if (distortion.strength < 0)
                    {
                        distortion.strength = 0;
                    }
                }

                if (ScanDistance < 0)
                {
                    _scanning = false;
                    ScanDistance = 0;
                    distortion.strength = 0;
                }

            }
        }
    }
    // End Demo Code

    void OnEnable()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.Depth;
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        EffectMaterial.SetVector("_WorldSpaceScannerPos", ScannerOrigin.position);
        EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
        RaycastCornerBlit(src, dst, EffectMaterial);
    }

    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
    {
        // Compute Frustum Corners
        float camFar = _camera.farClipPlane;
        float camFov = _camera.fieldOfView;
        float camAspect = _camera.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
        float camScale = topLeft.magnitude * camFar;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (_camera.transform.forward + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
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
