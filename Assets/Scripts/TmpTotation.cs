using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpTotation : MonoBehaviour
{
    [SerializeField] Vector3 rotationSpeed = new Vector3(0, 10, 0);

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
