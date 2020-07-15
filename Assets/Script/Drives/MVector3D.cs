using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVector3D
{
    public float x;
    public float y;
    public float z;

    public MVector3D(float m_x,float m_y, float m_z)
    {
        x = m_x;
        y = m_y;
        z = m_z;
    }

    public MVector3D(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public static MVector3D operator +(MVector3D a, MVector3D b)
    {
        return new MVector3D(a.x+b.x,a.y+b.y,a.z+b.z);
    }

    public static MVector3D operator -(MVector3D a, MVector3D b)
    {
        return new MVector3D(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static MVector3D operator *(MVector3D a, float d)
    {
        return new MVector3D(a.x * d, a.y * d, a.z * d);
    }

    public static MVector3D operator *(float d, MVector3D a)
    {
        return a * d;
    }

    public static MVector3D operator /(MVector3D a, float d)
    {
        return new MVector3D(a.x / d, a.y / d, a.z / d);
    }

    public static bool operator ==(MVector3D lhs, MVector3D rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
    }

    public static bool operator !=(MVector3D lhs, MVector3D rhs)
    {
        return !(lhs == rhs);
    }

    public Vector3 ToUnityVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2 ToUnityVector2()
    {
        return new Vector2(x, y);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
}
