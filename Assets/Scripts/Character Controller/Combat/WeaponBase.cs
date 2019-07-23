using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase
{
    public delegate void VelocityEventHandler(Vector3 velocity);
    public event VelocityEventHandler OnAddVelocityRequested;
    public event VelocityEventHandler OnSetVelocityRequested;

    protected RigAnimationController rigAnimationController;
    protected BurinkeruCharacterController characterController;

    public WeaponBase (RigAnimationController rigAnimationController, BurinkeruCharacterController characterController)
    {
        this.rigAnimationController = rigAnimationController;
        this.characterController = characterController;
    }

    public virtual bool CheckForInput (InputBuffer inputBuffer)
    {
        return false;
    }

    protected void addVelocity (Vector3 dVelocity)
    {
        OnAddVelocityRequested?.Invoke(dVelocity);
    }

    protected void setVelocity (Vector3 velocity)
    {
        OnSetVelocityRequested?.Invoke(velocity);
    }
}
