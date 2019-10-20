using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigWithGunAnimationController : RigAnimationController
{
    [SerializeField] Transform gunTransform;
    [SerializeField] Transform gunTransformWhileSliding;

    public Transform GunTransform
    {
        get { return gunTransform; }
    }

    public Transform GunTransformWhileSliding
    {
        get { return gunTransformWhileSliding; }
    }


    public void Reload()
    {
        animator.SetTrigger("Reload");
    }
}
