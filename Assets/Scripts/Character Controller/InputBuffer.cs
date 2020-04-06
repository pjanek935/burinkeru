using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputBuffer
{
    public delegate void OnNewInputInsertedEventHandler();
    public event OnNewInputInsertedEventHandler OnNewInputInserted;

    const int BUFFER_SIZE = 10;
    const int INPUT_INDEX = 0;
    const int COMMAND_INDEX = 1;
    const float interval = 0.25f;

    float lastBufferSaveTimeStamp = 0;
    bool[][] buffer;
    BurinkeruInputManager inputManager;
    bool ignoreSameInput = false;

    public InputBuffer ()
    {
        initBuffer();
        inputManager = BurinkeruInputManager.Instance;
    }

    void initBuffer ()
    {
        List<BurinkeruInputManager.InputCommand> allCommands =
            Enum.GetValues(typeof(BurinkeruInputManager.InputCommand)).Cast<BurinkeruInputManager.InputCommand>().ToList();
        buffer = new bool[BUFFER_SIZE][];

        for (int i = 0; i < buffer.Length; i ++)
        {
            buffer[i] = new bool[allCommands.Count];
        }
    }

    public void Clear ()
    {
        for (int i = 0; i < buffer.Length; i ++)
        {
            for (int j = 0; j < buffer [i].Length; j ++)
            {
                buffer[i][j] = false;
            }
        }
    }

    public void Update ()
    {
        bool[] currentDownCommands = inputManager.GetDownCommands();
        bool shouldInsertNewCommands = false;
        bool isInputDifferent = this.isInputDifferent(buffer[buffer.Length - 1], currentDownCommands);
        bool isInputEmpty = this.isInputEmpty(currentDownCommands);

        if ((ignoreSameInput || isInputDifferent) && ! isInputEmpty)
        {
            ignoreSameInput = false;
            shouldInsertNewCommands = true;
        }
        else if ((Time.time - lastBufferSaveTimeStamp) > interval)
        {
            shouldInsertNewCommands = true;
        }
        else if (isInputEmpty)
        {
            ignoreSameInput = true;
        }

        if (shouldInsertNewCommands)
        {
            lastBufferSaveTimeStamp = Time.time;
            insert(currentDownCommands);
        }
    }

    bool isInputEmpty (bool [] input)
    {
        bool result = true;

        for (int i = 0; i < input.Length; i ++)
        {
            if (input [i])
            {
                result = false;

                break;
            }
        }

        return result;
    }

    public bool Matches (CombatActionDefinition actionDefinition)
    {
        bool result = false;
    
        if (actionDefinition != null && buffer.Length >= actionDefinition.Count)
        {
            result = true;
            int helpIndex = buffer.Length - 1;

            for (int a = actionDefinition.Count - 1; a >= 0; a --)
            {
                List<BurinkeruInputManager.InputCommand> commands = actionDefinition.Get(a);

                for (int c = 0; c < commands.Count; c ++)
                {
                    if (! buffer [helpIndex] [(int) commands [c]])
                    {
                        result = false;

                        break;
                    }
                }

                if (! result)
                {
                    break;
                }

                helpIndex--;
            }
        }

        return result;
    }

    void insert (bool [] newInput)
    {
        for (int i = 1; i < buffer.Length; i++)
        {
            buffer[i - 1] = buffer[i];
        }

        buffer[buffer.Length - 1] = getCopy(newInput);

        OnNewInputInserted?.Invoke();
    }

    bool [] getCopy (bool [] input)
    {
        bool[] output = new bool[input.Length];

        for (int i = 0; i < input.Length; i ++)
        {
            output[i] = input[i];
        }

        return output;
    }

    bool isInputDifferent (bool [] input1, bool [] input2)
    {
        bool result = false;

        if (input1 != null && input2 != null && input1.Length == input2.Length)
        {
            if (input1 == input2)
            {
                result = false;
            }
            else
            {
                for (int i = 0; i < input1.Length; i ++)
                {
                    if (input1 [i] != input2 [i])
                    {
                        result = true;

                        break;
                    }
                }
            }
        }

        return result;
    }

    public string GetCurrentInputString (out bool isEmpty)
    {
        string result = "";
        isEmpty = true;

        for (int i = 0; i < buffer.Length; i ++)
        {
            result += "[";

            for (int j = 0; j < buffer [i].Length; j++)
            {
                if (buffer [i][j])
                {
                    result += ((BurinkeruInputManager.InputCommand)j).ToString () + ", ";
                    isEmpty = false;
                }
            }

            result += "]  ";
        }

        return result;
    }
}
