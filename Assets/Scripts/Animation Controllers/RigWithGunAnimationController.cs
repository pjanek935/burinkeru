using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigWithGunAnimationController : RigController
{
    [SerializeField] Transform gunTransform;
    [SerializeField] Transform gunTransformWhileSliding;
    [SerializeField] Transform clipOriginTransform;
    [SerializeField] GameObject clipGameObject;

    public Transform GunTransform
    {
        get { return gunTransform; }
    }

    public Transform GunTransformWhileSliding
    {
        get { return gunTransformWhileSliding; }
    }

    public Transform ClipOriginTransform
    {
        get { return clipOriginTransform; }
    }

    public GameObject ClipGameObject
    {
        get { return clipGameObject; }
    }

    public void Reload()
    {
        animator.SetTrigger("Reload");
    }
}
