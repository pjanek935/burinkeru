using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool IsActive
    {
        get;
        private set;
    }

    public virtual void Shoot (Vector3 origin, Vector3 forward, Vector3 up)
    {

    }
    public virtual float GetBaseSpeed (){ return 0f; }

    private void Update()
    {
        if (IsActive)
        {

        }
    }
}
