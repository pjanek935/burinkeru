using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableHitter : Hitter
{
    public delegate void SwordSlashHitterEventHandler(ActivatableHitter hitter);
    public event SwordSlashHitterEventHandler OnActivate;

    public bool IsActive
    {
        get;
        protected set;
    }

    public void Activate ()
    {
        OnActivate?.Invoke(this);
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsActive)
        {
            Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            onTriggerOrCollisionEnter(pos);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsActive)
        {
            Vector3 pos = collision.GetContact(0).point;
            onTriggerOrCollisionEnter(pos);
        }
    }

    void onTriggerOrCollisionEnter (Vector3 position)
    {
        if (this.HitterType == HitterType.SWORD)
        {
            
        }
    }
}
