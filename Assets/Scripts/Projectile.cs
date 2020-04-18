using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] Collider collider;
    [SerializeField] MeshRenderer model;
    [SerializeField] protected new Rigidbody rigidbody;

    public bool IsActive
    {
        get;
        private set;
    }

    protected void Awake()
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

    protected void setSpeed ()
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
        if (this.gameObject.activeInHierarchy)
        {
            //Waiting 2 frames so other hittables has a chance to get triggered
            StartCoroutine (waitNFramesAndTrigger (2, deactivate));
        }
        else
        {
            deactivate ();
        }
    }

    protected virtual void activate ()
    {
        if (model != null)
        {
            model.enabled = true;
        }

        this.gameObject.SetActive(true);
        IsActive = true;
        collider.enabled = true;
    }

    protected virtual void deactivate ()
    {
        if (model != null)
        {
            model.enabled = false;
        }

        rigidbody.velocity = Vector3.zero;
        IsActive = false;
        collider.enabled = false;
    }

    protected IEnumerator waitNFramesAndTrigger (int numberOfFrames, UnityAction action)
    {
        if (action != null)
        {
            for (int i = 0; i < numberOfFrames; i ++)
            {
                yield return new WaitForEndOfFrame ();
            }
           
            action.Invoke ();
        }
    }
}
