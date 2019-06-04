using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurinkeruInputManager : MonoBehaviour
{
    public enum InputCommand
    {
        NO_TYPE = 0,
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT,
        JUMP,
        CROUCH
    }

    static BurinkeruInputManager instance = null;

    List<ControllerWrapperBase> controllers = new List<ControllerWrapperBase>();

    public static BurinkeruInputManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject inputManagerObject = new GameObject("Burinkeru Input Manager");
                inputManagerObject.AddComponent<BurinkeruInputManager>();
            }

            return instance;
        }
    }

    public Vector2 GetLeftAxis(int controllerId = 0)
    {
        Vector2 result = Vector2.zero;

        if (controllers != null && controllers.Count > 0)
        {
            if (controllerId < 0 || controllerId > controllers.Count)
            {
                controllerId = 0;
            }

            result = controllers[controllerId].LeftAxis;
        }

        return result;
    }

    public Vector2 GetRightAxis(int controllerId = 0)
    {
        Vector2 result = Vector2.zero;

        if (controllers != null && controllers.Count > 0)
        {
            if (controllerId < 0 || controllerId > controllers.Count)
            {
                controllerId = 0;
            }

            result = controllers[controllerId].RightAxis;
        }

        return result;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            initControllers();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (this == instance)
        {
            instance = null;
        }
    }

    private void Update()
    {
        updateControllers();
    }

    void initControllers ()
    {
        KeyboardControllerWrapper keyboardControllerWrapper = new KeyboardControllerWrapper();
        controllers.Add(keyboardControllerWrapper);
    }

    void updateControllers ()
    {
        for (int i = 0; i < controllers.Count; i++)
        {
            controllers[i].Update();
        }
    }
}
