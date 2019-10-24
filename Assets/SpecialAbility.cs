using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialAbility
{
    protected CharacterComponents components;

    public bool IsActive
    {
        get;
        protected set;
    }

    public void Enter(CharacterComponents components)
    {
        this.components = components;
        IsActive = true;
        onEnter();
    }

    public void Exit()
    {
        IsActive = false;
        onExit();
    }

    protected abstract void onEnter();
    protected abstract void onExit();
}
