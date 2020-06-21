using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityTimeDrive : ITimeDrive
{
    public float GetDeltaTime()
    {
        return Time.deltaTime;
    }

    public float GetTime()
    {
        return Time.time;
    }

    public float GetTimeScale()
    {
        return Time.timeScale;
    }

    public float GetUnscaledDeltaTime()
    {
        return Time.unscaledDeltaTime;
    }

    public float GetUnscaledTime()
    {
        return Time.unscaledTime;
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
