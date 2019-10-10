using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAnimationController : MonoBehaviour
{
    const string LandTrigger = "Land";
    const string HardLandTrigger = "HardLand";

    [SerializeField] Animator animator = null;

    public void AnimateJump ()
    {
        //animator.SetTrigger("Jump");
    }

    public void AnimateLand()
    {
        animator.SetTrigger(LandTrigger);
    }

    public void AnimateHardLand()
    {
        animator.SetTrigger(HardLandTrigger);
    }

    public void AnimateWallRunLeft ()
    {

    }

    public void AnimateWallRunRight ()
    {
        //transform.DOLocalRotate (transform.eulerAngles + )
    }
}
