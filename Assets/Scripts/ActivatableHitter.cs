using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ActivatableHitter is a collider that can be handled differently, depending on
/// whether it is active or not. Eg. a sword is a collider that deals damage only when is
/// activated (in attack phase).
/// </summary>
public class ActivatableHitter : Hitter
{
    public delegate void SwordSlashHitterEventHandler(ActivatableHitter hitter, Hashtable parameters);
    public event SwordSlashHitterEventHandler OnActivate;

    public bool IsActive
    {
        get;
        protected set;
    }

    public Hashtable LastParameters
    {
        get;
        protected set;
    }

    public void Activate (Hashtable parameters)
    {
        LastParameters = parameters;
        OnActivate?.Invoke(this, parameters);
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

    }
}
