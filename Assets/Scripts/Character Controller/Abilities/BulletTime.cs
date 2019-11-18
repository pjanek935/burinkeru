using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : SpecialAbility
{
    protected override void onEnter()
    {
        TimeManager.Instance.StartSlowMotion();
        components.ScanEffect.StartEffect();
    }

    protected override void onExit()
    {
        TimeManager.Instance.EndSlowMotion();
        components.ScanEffect.EndEffect();;
    }
}
