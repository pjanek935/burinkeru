using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hittable component recives event and reacts to components with Hitter component.
/// </summary>
public class Hittable : MonoBehaviour
{
    public delegate void HitterEventHandler(Hitter hitter);
    public delegate void ActivatableHitterEventHandler (ActivatableHitter hitter);

    public event HitterEventHandler OnHitterEnter;
    public event HitterEventHandler OnHitterExit;
    public event ActivatableHitterEventHandler OnHitterActivated;

    protected List<ActivatableHitter> registeredHitters = new List<ActivatableHitter>();

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

            if (hitter.GetType() == typeof(ActivatableHitter))
            {
                ActivatableHitter activatableHitter = (ActivatableHitter)hitter;

                if (activatableHitter.IsActive)
                {
                    onHitterActivated(activatableHitter);
                }
                else if (! registeredHitters.Contains(activatableHitter))
                {
                    activatableHitter.OnActivate += onHitterActivated;
                    registeredHitters.Add(activatableHitter);
                }
            }
        }
    }

    void onHitterActivated (ActivatableHitter hitter)
    {
        OnHitterActivated?.Invoke(hitter);
        ShakeEffect.Instance.ShakeAndClampToGivenValue(0.5f);
        ParticlesManager.Instance.SwordOnHitParticleManager.ShootParticle();
    }

    private void OnTriggerExit(Collider other)
    {
        Hitter hitter = other.transform.gameObject.GetComponent<Hitter>();

        if (hitter != null)
        {
            OnHitterExit?.Invoke(hitter);

            if (hitter is ActivatableHitter)
            {
                ActivatableHitter activatableHitter = (ActivatableHitter)hitter;

                if (registeredHitters.Contains(activatableHitter))
                {
                    activatableHitter.OnActivate -= onHitterActivated;
                    int index = registeredHitters.IndexOf(activatableHitter);
                    registeredHitters.RemoveAt(index);
                }
            }
        }
    }

    private void OnDestroy()
    {
        unregisterAll();
    }

    void unregisterAll()
    {
        for (int i = 0; i < registeredHitters.Count; i++)
        {
            registeredHitters[i].OnActivate -= onHitterActivated;
        }

        registeredHitters.Clear();
    }
}
