using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    [SerializeField] GameObject toCut = null;

    private void Update()
    {
         if (Input.GetKeyDown (KeyCode.Return))
        {
            MeshCutter.Cut(toCut, this.transform.position, this.transform.up);
        }
    }
}
