using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPrinter : MonoBehaviour
{
    const int maxLines = 10;

    [SerializeField] Text text = null;

    Queue<string> lines = new Queue<string>();

    public void AddLine (string line)
    {
        lines.Enqueue(line);

        if (lines.Count > maxLines)
        {
            lines.Dequeue();
        }

        string text = "";

        foreach (string l in lines)
        {
            text += l + "\n";
        }

        this.text.text = text;
    }
}
