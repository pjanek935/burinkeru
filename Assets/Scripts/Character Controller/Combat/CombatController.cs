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
    KatanaWeapon katanaWeapon;
    RevolverWeapon revolverWeapon;
    InputBuffer inputBuffer;
    public bool IsChangingWeapon
    {
        get;
        private set;
    }

    private void Awake()
    {
        rigManager.OnNewRigSet += onNewRigSet;
    }

    void onNewRigSet ()
    {
        IsChangingWeapon = false;
    }

    private void Start()
    {
        init();
        setNewWeapon(katanaWeapon);
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

        if (!IsChangingWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (currentWeapon.GetType() != typeof(KatanaWeapon))
                {
                    setNewWeapon(katanaWeapon);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (currentWeapon.GetType() != typeof(RevolverWeapon))
                {
                    setNewWeapon(revolverWeapon);
                }
            }
        }
    }

    void init ()
    {
        inputBuffer = new InputBuffer();
        inputBuffer.OnNewInputInserted += onNewInputInserted;

        initAllWeapons();
    }

    void initAllWeapons ()
    {
        katanaWeapon = new KatanaWeapon();
        revolverWeapon = new RevolverWeapon();

        OnSetListenersToWeaponRequested?.Invoke(katanaWeapon);
        OnSetListenersToWeaponRequested?.Invoke(revolverWeapon);

        katanaWeapon.Init(rigManager, characterController, particlesManager);
        revolverWeapon.Init(rigManager, characterController, particlesManager);
    }

    void setNewWeapon (WeaponBase weapon)
    {
        IsChangingWeapon = true;
        currentWeapon = weapon;
        currentWeapon.Enter ();

        if (weapon.GetType () == typeof (KatanaWeapon))
        {
            rigManager.SwitchToKatana();
        }
        else if (weapon.GetType () == typeof (RevolverWeapon))
        {
            rigManager.SwitchToRevolver();
        }
    }

    void onNewInputInserted ()
    {
        bool isEmpty;
        inputPrinter.AddLine(inputBuffer.GetCurrentInputString (out isEmpty));
    }
}
