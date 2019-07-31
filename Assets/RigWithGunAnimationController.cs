using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigWithGunAnimationController : RigAnimationController
{
    public void Reload()
    {
        animator.SetTrigger("Reload");
    }
}
