using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitterType
{
    BULLET,
    SWORD,
}

public class Hitter : MonoBehaviour
{
    [SerializeField] HitterType hitterType;

    public HitterType HitterType
    {
        get { return hitterType; }
    }
}
