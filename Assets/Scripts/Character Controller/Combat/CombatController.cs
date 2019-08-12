using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public delegate void RequestSetListenersToWeaponEventHandler(WeaponBase weapon);
    public event RequestSetListenersToWeaponEventHandler OnSetListenersToWeaponRequested;

    [SerializeField] InputPrinter inputPrinter = null;
    [SerializeField] RigManager rigManager;
    [SerializeField] BurinkeruCharacterController characterController;
    [SerializeField] ParticlesManager particlesManager;

    WeaponBase currentWeapon;
    InputBuffer inputBuffer;
    bool changingWeapon = false;

    private void Awake()
    {
        rigManager.OnNewRigSet += onNewRigSet;
    }

    void onNewRigSet ()
    {
        currentWeapon.Init(rigManager, characterController, particlesManager);
        changingWeapon = false;
    }

    private void Start()
    {
        inputBuffer = new InputBuffer();
        inputBuffer.OnNewInputInserted += onNewInputInserted;

        setNewWeapon(new KatanaWeapon ());
    }

    void setNewWeapon (WeaponBase weapon)
    {
        changingWeapon = true;
        currentWeapon = weapon;
        OnSetListenersToWeaponRequested?.Invoke(weapon);
        
        if (weapon.GetType () == typeof (KatanaWeapon))
        {
            rigManager.SwitchToKatana();
        }
        else if (weapon.GetType () == typeof (RevolverWeapon))
        {
            rigManager.SwitchToRevolver();
        }
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


        if (!changingWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (currentWeapon.GetType () != typeof (KatanaWeapon))
                {
                    setNewWeapon(new KatanaWeapon());
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (currentWeapon.GetType() != typeof(RevolverWeapon))
                {
                    setNewWeapon(new RevolverWeapon());
                }
            }
        }
    }

    void onNewInputInserted ()
    {
        bool isEmpty;
        inputPrinter.AddLine(inputBuffer.GetCurrentInputString (out isEmpty));
    }
}
