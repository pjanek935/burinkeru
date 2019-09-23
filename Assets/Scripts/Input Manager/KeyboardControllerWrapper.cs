using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControllerWrapper : ControllerWrapperBase
{
    public override void Update()
    {
        updateKeys();
        updateAxes();
    }

    void updateKeys ()
    {
        BurinkeruInputManager.InputCommand command = BurinkeruInputManager.InputCommand.NO_TYPE;
        KeyCode mappedKey = KeyCode.None;

        for (int c = 0; c < pressCommands.Length; c++)
        {
            command = (BurinkeruInputManager.InputCommand)c;

            if (command == BurinkeruInputManager.InputCommand.ATTACK)
            {
                downCommands[c] = Input.GetMouseButtonDown(0);
                upCommands[c] = Input.GetMouseButtonUp(0);

                if (downCommands[c])
                {
                    pressCommands[c] = true;
                }
                else if (upCommands[c])
                {
                    pressCommands[c] = false;
                }
            }
            else if (command == BurinkeruInputManager.InputCommand.BLINK)
            {
                downCommands[c] = Input.GetMouseButtonDown(1);
                upCommands[c] = Input.GetMouseButtonUp(1);

                if (downCommands[c])
                {
                    pressCommands[c] = true;
                }
                else if (upCommands[c])
                {
                    pressCommands[c] = false;
                }
            }
            else if (command != BurinkeruInputManager.InputCommand.NO_TYPE)
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
        float leftX = pressCommands[(int)BurinkeruInputManager.InputCommand.RIGHT] ? 1f : 0f;
        leftX -= pressCommands[(int)BurinkeruInputManager.InputCommand.LEFT] ? 1f : 0f;
        float leftY = pressCommands[(int)BurinkeruInputManager.InputCommand.FORWARD] ? 1f : 0f;
        leftY -= pressCommands[(int)BurinkeruInputManager.InputCommand.BACKWARD] ? 1f : 0f;
        LeftAxis = new Vector2(leftX, leftY);
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

            case BurinkeruInputManager.InputCommand.RUN:

                result = KeyCode.LeftShift;

                break;

            case BurinkeruInputManager.InputCommand.RELOAD:

                result = KeyCode.R;

                break;
        }

        return result;
    }
}
