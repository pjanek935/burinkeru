using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cuttable components' meshes can be cut by hitters marked as HitterType.BLADE.
/// </summary>
[RequireComponent (typeof (Hittable))]
public class Cuttable : MonoBehaviour
{
    Hittable hittable;
    int childIndex = 0;
    bool isFrozen = false;

    const int maxChildren = 100;

    private void Awake()
    {
        hittable = GetComponent<Hittable>();
        TimeManager.Instance.OnTimeFactorChanged += onTimeFactorChanged;

        if (hittable != null)
        {
            hittable.OnHitterActivated += onHitterActivated;
        }
    }

    private void OnDestroy()
    {
        if (hittable != null)
        {
            hittable.OnHitterActivated -= onHitterActivated;
            TimeManager.Instance.OnTimeFactorChanged -= onTimeFactorChanged;
        }
    }

    void onHitterActivated (ActivatableHitter hitter, Hashtable parameters)
    {
        if (hitter.HitterType == HitterType.BLADE)
        {
            cut(hitter.transform);
        }
    }

    public void SetChildIndex (int index)
    {
        this.childIndex = index;
    }

    void onTimeFactorChanged()
    {
        if (! TimeManager.Instance.IsSlowMotionOn && isFrozen)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
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
                newGameObjects[i].AddComponent<BoxCollider>();
                newGameObjects[i].AddComponent<Hittable>();
                Cuttable cuttable = newGameObjects[i].AddComponent<Cuttable>();
                cuttable.SetChildIndex(this.childIndex + newGameObjects.Length);

                rb.isKinematic = TimeManager.Instance.IsSlowMotionOn;
                cuttable.isFrozen = TimeManager.Instance.IsSlowMotionOn;
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
