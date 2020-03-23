using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject model;
    [SerializeField] protected new Rigidbody rigidbody;

    public bool IsActive
    {
        get;
        private set;
    }

    private void Awake()
    {
        TimeManager.Instance.OnTimeFactorChanged += onTimeFactorChanged;
    }

    void onTimeFactorChanged ()
    {
        if (IsActive)
        {
            setSpeed();
        }
    }

    public virtual void Shoot (Vector3 origin, Vector3 forward, Vector3 up)
    {
        this.transform.position = origin;
        this.transform.rotation = Quaternion.LookRotation(up, forward);
        this.transform.position = origin;

        activate();
        setSpeed();
    }

    void setSpeed ()
    {
        float speed = GetBaseSpeed();

        if (TimeManager.Instance.IsSlowMotionOn)
        {
            speed *= GetSlowMoFactor();
        }

        Vector3 deltaMove = this.transform.forward * speed;
        rigidbody.velocity = deltaMove;
    }

    public virtual float GetBaseSpeed (){ return 10f; }

    public virtual float GetSlowMoFactor() { return 0.1f; }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        deactivate();
    }

    protected virtual void activate ()
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
