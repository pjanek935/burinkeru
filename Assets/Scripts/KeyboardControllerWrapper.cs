using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControllerWrapper : ControllerWrapperBase
{
    public override void Update()
    {
        updateAxes();

        updateKeys();
    }

    void updateKeys ()
    {
        BurinkeruInputManager.InputCommand command = BurinkeruInputManager.InputCommand.NO_TYPE;
        KeyCode mappedKey = KeyCode.None;

        for (int c = 0; c < pressCommands.Length; c++)
        {
            command = (BurinkeruInputManager.InputCommand)c;

            if (command != BurinkeruInputManager.InputCommand.NO_TYPE)
            {
                mappedKey = GetDefaultKeyMapping(command);
                downCommands[c] = Input.GetKeyDown(mappedKey);
                upCommands[c] = Input.GetKeyUp(mappedKey);

                if (downCommands[c])
                {
                    pressCommands[c] = true;
                }
                else if (upCommands[c])
                {
                    pressCommands[c] = false;
                }
            }
        }
    }

    void updateAxes ()
    {
        RightAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public static KeyCode GetDefaultKeyMapping (BurinkeruInputManager.InputCommand inputCommand)
    {
        KeyCode result = KeyCode.None;

        switch (inputCommand)
        {
            case BurinkeruInputManager.InputCommand.FORWARD:

                result = KeyCode.W;

                break;

            case BurinkeruInputManager.InputCommand.BACKWARD:

                result = KeyCode.S;

                break;

            case BurinkeruInputManager.InputCommand.LEFT:

                result = KeyCode.A;

                break;

            case BurinkeruInputManager.InputCommand.RIGHT:

                result = KeyCode.D;

                break;

            case BurinkeruInputManager.InputCommand.JUMP:

                result = KeyCode.Space;

                break;

            case BurinkeruInputManager.InputCommand.CROUCH:

                result = KeyCode.LeftControl;

                break;
        }

        return result;
    }
}
