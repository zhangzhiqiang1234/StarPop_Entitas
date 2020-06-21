using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInputDrive : IInputDrive
{
    public bool GetMouseButtonDown(int keyCode)
    {
        return Input.GetMouseButtonDown(keyCode);
    }

    public MVector3D MousePosition()
    {
        return new MVector3D(Input.mousePosition);
    }

    public MVector3D ScreenToWorldPoint(MVector3D vector3D)
    {
        Vector3 vector3 = vector3D.ToUnityVector3();
        return new MVector3D(Camera.main.ScreenToWorldPoint(vector3));
    }
}
