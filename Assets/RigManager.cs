using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigManager : MonoBehaviour
{
    [SerializeField] RigWithKatanaAnimationController rigWithKatana;
    [SerializeField] RigWithGunAnimationController rigWithRevolver;

    RigAnimationController currentRig;
    RigAnimationController nextRig = null;
    

    private void Awake()
    {
        currentRig = rigWithKatana;

        rigWithRevolver.OnHideEnded += onHideEnded;
        rigWithKatana.OnHideEnded += onHideEnded;
    }

    public void SwitchToKatana ()
    {
        if (currentRig != rigWithKatana)
        {
            currentRig.Hide();
            nextRig = rigWithKatana;
        }
    }

    public void SwitchToRevolver ()
    {
        if (currentRig != rigWithRevolver)
        {
            currentRig.Hide();
            nextRig = rigWithRevolver;
        }
    }

    void onHideEnded ()
    {
        currentRig.gameObject.SetActive(false);
        nextRig.gameObject.SetActive(true);
        currentRig = nextRig;
        nextRig = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown (KeyCode.Alpha1))
        {
            SwitchToKatana();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToRevolver(); 
        }
    }
}
