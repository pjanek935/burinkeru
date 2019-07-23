using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ControllerWrapperBase
{
    public int ID
    {
        get;
        protected set;
    }

    public ControllerWrapperBase ()
    {
        initCommandArrays();
    }

    protected bool[] pressCommands; //Commands that are beeing held down; true in frame when key was pressed, true until released (false in frame when key was released)
    protected bool[] downCommands; //True only in frame when key was pressed
    protected bool[] upCommands; //True only in frame when key was released

    public Vector2 LeftAxis
    {
        get;
        protected set;
    }

    public Vector2 RightAxis
    {
        get;
        protected set;
    }

    public bool [] PressCommands
    {
        get { return pressCommands; }
    }

    public bool [] DownCommands
    {
        get { return downCommands; }
    }

    public bool [] UpCommads
    {
        get { return upCommands; }
    }

    public bool IsCommandDown (BurinkeruInputManager.InputCommand inputCommand)
    {
        return downCommands[(int) inputCommand];
    }

    public bool IsCommandPressed(BurinkeruInputManager.InputCommand inputCommand)
    {
        return pressCommands[(int)inputCommand];
    }

    public bool IsCommandUp(BurinkeruInputManager.InputCommand inputCommand)
    {
        return upCommands[(int)inputCommand];
    }

    public abstract void Update();

    void initCommandArrays ()
    {
        List <BurinkeruInputManager.InputCommand> allCommands =
            Enum.GetValues(typeof(BurinkeruInputManager.InputCommand)).Cast <BurinkeruInputManager.InputCommand> ().ToList ();
        pressCommands = new bool[allCommands.Count];
        downCommands = new bool[allCommands.Count];
        upCommands = new bool[allCommands.Count];
    }
}
