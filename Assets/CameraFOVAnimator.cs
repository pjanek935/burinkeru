using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVAnimator : MonoBehaviour
{
    [SerializeField] new Camera camera;

    public float CurrentFOV
    {
        get { return camera.fieldOfView; }
    }

    public void SetFOV (float fov, float speed = 0.45f)
    {
        DOTween.To(() => camera.fieldOfView, x => camera.fieldOfView = x, fov, speed);
    }

    public void ResetToDefault (float speed = 0.45f)
    {
        DOTween.To(() => camera.fieldOfView, x => camera.fieldOfView = x, 60f, speed);
    }
}
