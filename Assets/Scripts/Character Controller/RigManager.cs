using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigManager : MonoBehaviour
{
    public delegate void OnNewRigSetEventHandler();
    public event OnNewRigSetEventHandler OnNewRigSet;

    [SerializeField] RigWithKatanaController rigWithKatana;
    [SerializeField] RigWithGunAnimationController rigWithRevolver;

    RigController currentRig;
    RigController nextRig = null;

    public RigController CurrentRig
    {
        get { return currentRig; }
        set { CurrentRig = value; }
    }

    public RigWithKatanaController RigWithKatana
    {
        get { return rigWithKatana; }
        set { rigWithKatana = value; }
    }

    public RigWithGunAnimationController RigWithRevolver
    {
        get { return rigWithRevolver; }
        set { rigWithRevolver = value; }
    }

    private void Awake()
    {
        rigWithRevolver.OnHideEnded += onHideEnded;
        rigWithKatana.OnHideEnded += onHideEnded;
        TimeManager.Instance.OnTimeFactorChanged += onTimeFactorChanged;
    }

    public void SwitchToKatana ()
    {
        if (currentRig != rigWithKatana)
        {
            if (currentRig != null)
            {
                currentRig.Hide();
                nextRig = rigWithKatana;
            }
            else
            {
                currentRig = rigWithKatana;
                currentRig.gameObject.SetActive(true);
                nextRig = null;
                OnNewRigSet?.Invoke();
            }
        }
    }

    public void SwitchToRevolver ()
    {
        if (currentRig != rigWithRevolver)
        {
            if (currentRig != null)
            {
                currentRig.Hide();
                nextRig = rigWithRevolver;
            }
            else
            {
                currentRig = rigWithRevolver;
                currentRig.gameObject.SetActive(true);
                nextRig = null;
                OnNewRigSet?.Invoke();
            }
        }
    }

    void onHideEnded ()
    {
        currentRig.gameObject.SetActive(false);
        nextRig.gameObject.SetActive(true);
        currentRig = nextRig;
        nextRig = null;

        OnNewRigSet?.Invoke();
    }

    void onTimeFactorChanged ()
    {
        float timeFactor = 1f / TimeManager.Instance.TimeFactor;
        rigWithKatana.SetTimeFactor(timeFactor);
        rigWithRevolver.SetTimeFactor(timeFactor);
    }
}
