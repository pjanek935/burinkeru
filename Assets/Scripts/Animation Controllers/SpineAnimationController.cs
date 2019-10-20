using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAnimationController : MonoBehaviour
{
    float rotation = 10f;
    float duration = 0.3f;

    public void WallRunLeft ()
    {
        transform.DOLocalRotate(new Vector3(0, 0, -rotation), duration, RotateMode.Fast);
    }

    public void WallRunRight()
    {
        transform.DOLocalRotate(new Vector3(0, 0, rotation), duration, RotateMode.Fast);
    }

    public void Default()
    {
        transform.DOLocalRotate(Vector3.zero, duration*1.5f, RotateMode.Fast);
    }
}
