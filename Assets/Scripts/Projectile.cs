using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject model;
    [SerializeField] Rigidbody rigidbody;

    public bool IsActive
    {
        get;
        private set;
    }

    public virtual void Shoot (Vector3 origin, Vector3 forward, Vector3 up)
    {
        this.transform.position = origin;
        this.transform.rotation = Quaternion.LookRotation(up, forward);
        this.transform.position = origin;

        activate();

        float speed = GetBaseSpeed();
        Vector3 deltaMove = this.transform.forward * speed;
        rigidbody.velocity = deltaMove;
    }

    public virtual float GetBaseSpeed (){ return 10f; }

    //private void Update()
    //{
    //    if (IsActive)
    //    {

    //    }
    //}

    protected virtual void OnCollisionEnter(Collision collision)
    {
        deactivate();
    }

    protected void activate ()
    {
        if (model != null)
        {
            model.gameObject.SetActive(true);
        }

        this.gameObject.SetActive(true);
        IsActive = true;
    }

    protected void deactivate ()
    {
        if (model != null)
        {
            model.gameObject.SetActive(false);
        }

        rigidbody.velocity = Vector3.zero;
        IsActive = false;
    }
}
