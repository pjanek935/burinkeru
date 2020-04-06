using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : CharacterStateBase
{
    protected BurinkeruInputManager inputManager;
    protected CharacterComponents components;

    public BurinkeruCharacterController BurinkeruCharacterController
    {
        get { return (BurinkeruCharacterController) characterController; }
    }

    public void Enter (BurinkeruInputManager inputManager, BurinkeruCharacterController parent, CharacterComponents components)
    {
        this.inputManager = inputManager;
        this.components = components;

        base.Enter (parent);
    }

    protected virtual void switchCrouch()
    {
        BurinkeruCharacterController burinkeruCharacterController = (BurinkeruCharacterController) characterController;

        if (burinkeruCharacterController.IsCrouching)
        {
            burinkeruCharacterController.ExitCrouch();
        }
        else
        {
            burinkeruCharacterController.EnterCrouch();
        }
    }
}
