using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitterType
{
    PROJECTILE,
    BLADE,
}

/// <summary>
/// Hitter component can interact with Hittable components.
/// This component should be attached to a objects such as bullets,
/// so hittable components can react to collision or trigger events with it.
/// </summary>
public class Hitter : MonoBehaviour
{
    [SerializeField] HitterType hitterType;

    public HitterType HitterType
    {
        get { return hitterType; }
    }
}
