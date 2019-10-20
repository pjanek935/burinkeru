using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigDelayedRotation : MonoBehaviour
{
    [SerializeField] Transform transformToFollow;
    // Update is called once per frame
    void Update()
    {
        if (transformToFollow != null)
        {
            this.transform.position = transformToFollow.position;
            this.transform.rotation = transformToFollow.rotation;
        }
    }
}
