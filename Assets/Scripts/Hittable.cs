using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    public delegate void HittableEventHandler(Hitter hitter);
    public event HittableEventHandler OnHitterEnter;
    public event HittableEventHandler OnHitterExit;

    private void Awake()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        Hitter hitter = other.GetComponent<Hitter>();

        if (hitter != null)
        {
            OnHitterEnter?.Invoke(hitter);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Hitter hitter = collision.transform.gameObject.GetComponent<Hitter>();

        if (hitter != null)
        {
            OnHitterEnter?.Invoke(hitter);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Hitter hitter = collision.transform.gameObject.GetComponent<Hitter>();

        if (hitter != null)
        {
            OnHitterExit?.Invoke(hitter);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Hitter hitter = other.transform.gameObject.GetComponent<Hitter>();

        if (hitter != null)
        {
            OnHitterEnter?.Invoke(hitter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Hitter hitter = other.transform.gameObject.GetComponent<Hitter>();

        if (hitter != null)
        {
            OnHitterExit?.Invoke(hitter);
        }
    }
}
