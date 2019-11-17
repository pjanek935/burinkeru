using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Hittable))]
public class Cuttable : MonoBehaviour
{
    Hittable hittable;
    int childIndex = 0;
    List<ActivatableHitter> registeredHitters = new List<ActivatableHitter>();

    const int maxChildren = 10;

    private void Awake()
    {
        hittable = GetComponent<Hittable>();

        if (hittable != null)
        {
            hittable.OnHitterEnter += onHitterEnter;
            hittable.OnHitterExit += onHitterExit;
        }
    }

    private void OnDestroy()
    {
        if (hittable != null)
        {
            hittable.OnHitterEnter -= onHitterEnter;
            hittable.OnHitterExit -= onHitterExit;
        }

        unregisterAll();
    }

    public void SetChildIndex (int index)
    {
        this.childIndex = index;
    }

    void onHitterEnter (Hitter hitter)
    {
        if (hitter.HitterType == HitterType.SWORD && hitter is ActivatableHitter)
        {
            ActivatableHitter activatableHitter = (ActivatableHitter)hitter;

            if (! registeredHitters.Contains (activatableHitter))
            {
                activatableHitter.OnActivate += cut;
                registeredHitters.Add(activatableHitter);
            }
        }
    }

    void onHitterExit (Hitter hitter)
    {
        if (hitter.HitterType == HitterType.SWORD && hitter is ActivatableHitter)
        {
            ActivatableHitter activatableHitter = (ActivatableHitter)hitter;

            if (registeredHitters.Contains(activatableHitter))
            {
                activatableHitter.OnActivate -= cut;
                registeredHitters.Remove(activatableHitter);
            }
        }
    }

    void unregisterAll ()
    {
        for (int i = 0; i < registeredHitters.Count; i ++)
        {
            registeredHitters[i].OnActivate -= cut;
        }

        registeredHitters.Clear();
    }

    void cut(Transform plane)
    {
        if (! canBeCut ())
        {
            return;
        }

        List<GameObject> newObjectsList = new List<GameObject>();
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        GameObject[] newGameObjects = this.gameObject.SliceInstantiate(plane.position, plane.transform.up, mat);
        bool wasCut = false;

        if (newGameObjects != null && newGameObjects.Length > 0)
        {
            wasCut = true;

            for (int i = 0; i < newGameObjects.Length; i++)
            {
                newObjectsList.Add(newGameObjects[i]);
                Rigidbody rb = newGameObjects[i].AddComponent<Rigidbody>();
                //rb.isKinematic = true;
                newGameObjects[i].AddComponent<BoxCollider>();
                newGameObjects[i].AddComponent<Hittable>();
                Cuttable cuttable = newGameObjects[i].AddComponent<Cuttable>();
                cuttable.SetChildIndex(this.childIndex + newGameObjects.Length);
            }
        }

        if (wasCut)
        {
            Destroy(this.gameObject);
        }
    }

    bool canBeCut ()
    {
        bool result = false;

        if (this.gameObject != null &&
            this.gameObject.activeInHierarchy &&
            childIndex < maxChildren)
        {
            result = true;
        }

        return result;
    }
}
