
using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCutter : MonoBehaviour
{
    [SerializeField] List<Transform> planes = new List<Transform>();

    [SerializeField] List<GameObject> objects = new List<GameObject>();

    private void Awake()
    {
        //objects.Add(toCut);
    }

    public void Cut (int planeIndex)
    {
        List<GameObject> newObjectsList = new List<GameObject>();

        for (int i = objects.Count-1; i >= 0; i --)
        {
            if (planeIndex >= 0 && planeIndex < planes.Count)
            {
                GameObject[] newGameObjects = objects[i].SliceInstantiate(planes [planeIndex].transform.position, planes [planeIndex].transform.up);

                if (newGameObjects != null && newGameObjects.Length > 0)
                {
                    for (int j = 0; j < newGameObjects.Length; j++)
                    {
                        newObjectsList.Add(newGameObjects[j]);
                        Rigidbody rb = newGameObjects[j].AddComponent<Rigidbody>();
                        newGameObjects[j].AddComponent<BoxCollider>();
                        newGameObjects[j].AddComponent<Hittable>();

                        rb.AddForce(Vector3.up*5, ForceMode.Impulse);
                    }

                    Destroy(objects[i]);
                    objects.RemoveAt(i);
                }
            }
            
        }

        objects.AddRange(newObjectsList);
    }
}
