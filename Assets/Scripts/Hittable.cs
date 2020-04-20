using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

/// <summary>
/// Hittable component recives event and reacts to components with Hitter component.
/// </summary>
public class Hittable : MonoBehaviour
{
    public delegate void HitterEventHandler(Hitter hitter);
    public delegate void ActivatableHitterEventHandler (ActivatableHitter hitter, Hashtable parameters);

    public event HitterEventHandler OnHitterEnter;
    public event HitterEventHandler OnHitterExit;
    public event ActivatableHitterEventHandler OnHitterActivated;

    [SerializeField] bool sendMessageEnabled = true;
    [SerializeField] SenderType senderType = SenderType.OBJECT;

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

        if (hitter != null && hitter.gameObject.activeInHierarchy)
        {
            OnHitterEnter?.Invoke(hitter);

            if (hitter.GetType() == typeof(ActivatableHitter))
            {
                ActivatableHitter activatableHitter = (ActivatableHitter)hitter;

                if (activatableHitter.IsActive)
                {
                    onHitterActivated(activatableHitter, activatableHitter.LastParameters);
                }
                else if (! registeredHitters.Contains(activatableHitter))
                {
                    activatableHitter.OnActivate += onHitterActivated;
                    registeredHitters.Add(activatableHitter);
                }
            }
        }
    }

    void onHitterActivated (ActivatableHitter hitter, Hashtable parameters)
    {
        OnHitterActivated?.Invoke(hitter, parameters);

        if (sendMessageEnabled)
        {
            Hashtable feedbackParameters = new Hashtable ();
            feedbackParameters.Add (ParameterType.SENDER_TYPE, senderType);
            hitter.SendMessage (feedbackParameters);
        }
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
