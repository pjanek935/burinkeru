using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableHitter : Hitter
{
    public delegate void SwordSlashHitterEventHandler(Transform transform);
    public event SwordSlashHitterEventHandler OnActivate;

    public void Activate ()
    {
        OnActivate?.Invoke(this.transform);
    }
}
