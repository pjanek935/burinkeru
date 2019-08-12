using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigWithGunAnimationController : RigAnimationController
{
    [SerializeField] Transform gunTransform;

    public Transform GunTransform
    {
        get { return gunTransform; }
    }

    public void Reload()
    {
        animator.SetTrigger("Reload");
    }
}
