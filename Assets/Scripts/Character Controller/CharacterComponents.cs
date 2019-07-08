using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponents : MonoBehaviour
{
    [SerializeField] Camera fppCamera = null;
    [SerializeField] GameObject body = null;
    [SerializeField] HeadAnimationController head = null;
    [SerializeField] CapsuleCollider capsuleCollider = null;
    [SerializeField] RigAnimationController rigAnimationController = null;

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

    public RigAnimationController RigAnimationController
    {
        get { return rigAnimationController; }
    }
}
