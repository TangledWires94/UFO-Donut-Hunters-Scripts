using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging : MonoBehaviour
{
    public void WriteToConsole()
    {
        Debug.Log("Debug");
    }

    public void WritetoConsole(string debugText)
    {
        Debug.Log(debugText);
    }
}
