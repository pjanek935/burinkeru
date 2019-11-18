using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager
{
    private static TimeManager instance = null;

    const float SLOW_MO_FACTOR = 0.25f;
    const float FIXED_DELTA_TIME = 0.02f;

    public static TimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimeManager();
            }

            return instance;
        }
    }

    public UnityAction OnTimeFactorChanged;

    public float TimeFactor
    {
        get;
        private set;
    }

    public bool IsSlowMotionOn
    {
        get;
        private set;
    }

    private TimeManager ()
    {
        TimeFactor = 1f;
    }

    public void StartSlowMotion ()
    {
        IsSlowMotionOn = true;
        Time.timeScale = SLOW_MO_FACTOR;
        Time.fixedDeltaTime = FIXED_DELTA_TIME * Time.timeScale;
        changeTimeFactor(SLOW_MO_FACTOR);
    }

    public void EndSlowMotion ()
    {
        IsSlowMotionOn = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = FIXED_DELTA_TIME;
        changeTimeFactor(1f);
    }

    void changeTimeFactor (float timeFactor)
    {
        this.TimeFactor = timeFactor;
        OnTimeFactorChanged?.Invoke();
    }
}
