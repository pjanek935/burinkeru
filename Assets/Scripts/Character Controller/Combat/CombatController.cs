using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] InputPrinter inputPrinter = null;

    WeaponBase currentWeapon;
    InputBuffer inputBuffer;

    private void Awake()
    {
        inputBuffer = new InputBuffer();
        inputBuffer.OnNewInputInserted += onNewInputInserted;
    }

    public void SetNewWeapon (WeaponBase weapon)
    {
        currentWeapon = weapon;
    }

    private void Update()
    {
        inputBuffer.Update();

        if (currentWeapon != null)
        {
            bool actionRequested = currentWeapon.CheckForInput(inputBuffer);

            if (actionRequested)
            {
                inputBuffer.Clear();
            }
        }
    }

    void onNewInputInserted ()
    {
        bool isEmpty;
        inputPrinter.AddLine(inputBuffer.GetCurrentInputString (out isEmpty));
    }
}
