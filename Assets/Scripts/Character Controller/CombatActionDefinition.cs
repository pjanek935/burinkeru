using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionDefinition
{
    List<List<BurinkeruInputManager.InputCommand>> actionDefinition = new List<List<BurinkeruInputManager.InputCommand>>();

    public int Count
    {
        get { return actionDefinition.Count; }
    }

    public List <BurinkeruInputManager.InputCommand> Get (int index)
    {
        List<BurinkeruInputManager.InputCommand> result = null;

        if (index >= 0 && index < Count)
        {
            result = actionDefinition[index];
        }

        return result;
    }

    public void Add (List <BurinkeruInputManager.InputCommand> commands)
    {
        actionDefinition.Add(commands);
    }

    public void Add (params BurinkeruInputManager.InputCommand[] commands)
    {
        List<BurinkeruInputManager.InputCommand> list = new List<BurinkeruInputManager.InputCommand>();
        list.AddRange(commands);
        actionDefinition.Add(list);
    }

    public void Add (BurinkeruInputManager.InputCommand command)
    {
        actionDefinition.Add(new List<BurinkeruInputManager.InputCommand>() { command});
    }
}
