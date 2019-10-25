using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponents : MonoBehaviour
{
    [SerializeField] Camera fppCamera = null;
    [SerializeField] GameObject body = null;
    [SerializeField] HeadAnimationController head = null;
    [SerializeField] CapsuleCollider capsuleCollider = null;
    [SerializeField] RigManager rigManager = null;
    [SerializeField] CameraFOVAnimator cameraFOVAnimator;
    [SerializeField] SpineAnimationController spineAnimationController;
    [SerializeField] ScanEffect scanEffect;
    [SerializeField] BlinkShadePostProcessEffect blinkShadePostProcessEffect;

    public Camera FPPCamera
    {
        get { return fppCamera; }
    }

    public GameObject Body
    {
        get { return body; }
    }

    public HeadAnimationController Head
    {
        get { return head; }
    }

    public CapsuleCollider CapsuleCollider
    {
        get { return capsuleCollider; }
    }

    public RigManager RigManager
    {
        get { return rigManager; }
    }

    public CameraFOVAnimator CameraFOVAnimator
    {
        get { return cameraFOVAnimator; }
    }

    public SpineAnimationController SpineAnimationController
    {
        get { return spineAnimationController; }
    }

    public ScanEffect ScanEffect
    {
        get { return scanEffect; }
    }

    public BlinkShadePostProcessEffect BlinkShadePostProcessEffect
    {
        get { return blinkShadePostProcessEffect; }
    }
}
