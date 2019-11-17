using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : SpecialAbility
{
    const float SLOW_MO_FACTOR = 0.25f;
    const float FIXED_DELTA_TIME = 0.02f;

    protected override void onEnter()
    {
        Time.timeScale = SLOW_MO_FACTOR;
        Time.fixedDeltaTime = FIXED_DELTA_TIME * Time.timeScale;
        components.ScanEffect.StartEffect();
        components.RigManager.SetTimeFactor((1f/SLOW_MO_FACTOR));
    }

    protected override void onExit()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = FIXED_DELTA_TIME;
        components.ScanEffect.EndEffect();
        components.RigManager.SetTimeFactor(1f);
    }
}
