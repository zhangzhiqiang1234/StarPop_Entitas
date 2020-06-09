using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectsBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CheckResources();
    }

    private void CheckResources()
    {
        bool isSupported = CheckSupported();
        if (!isSupported)
        {
            NotSupported();
        }
    }

    private bool CheckSupported()
    {
        if (SystemInfo.supportsImageEffects)
        {
            return true;
        }
        Debug.LogWarning("This platform does not support image effects");
        return false;
    }

    protected void NotSupported()
    {
        enabled = false;
    }

    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if (shader == null)
        {
            return null;
        }
        if (!shader.isSupported)
        {
            return null;
        }
        if (material && material.shader == shader)
        {
            return material;
        }
        else
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if (material)
            {
                return material;
            }
            else
            {
                return null;
            }
        }
    }
}
