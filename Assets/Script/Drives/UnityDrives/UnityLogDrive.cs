using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLogDrive : ILogDrive
{
    public void LogMessage(string log)
    {
        Debug.Log(log);
    }
}
