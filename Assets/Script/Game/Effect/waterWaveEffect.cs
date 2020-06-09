using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterWaveEffect : PostEffectsBase
{
    public Shader shader;

    //距离系数    
    public float distanceFactor = 60.0f;
    //时间系数    
    public float timeFactor = -30.0f;
    //sin函数结果系数    
    public float totalFactor = 1.0f;


    //波纹宽度

    public float waveWidth = 0.3f;
    //波纹扩散的速度
    public float waveSpeed = 0.3f;

    //波纹范围
    [Range(0, 1)]
    public float rectWave = 1;

    private float waveStartTime = 0;
    private Vector4 startPos = new Vector4(0.5f, 0.5f, 0, 0);

    private Material _material = null;
    private Material material
    {
        get
        {
            _material = CheckShaderAndCreateMaterial(shader, _material);
            return _material;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null && waveStartTime != 0)
        {
            float curWaveDis = waveSpeed * (Time.time - waveStartTime);

            material.SetFloat("_distanceFactor", distanceFactor);
            material.SetFloat("_totalFactor", totalFactor);
            material.SetFloat("_timeFactor", timeFactor);
            material.SetFloat("_curWaveDis", curWaveDis);
            material.SetFloat("_waveWidth", waveWidth);
            material.SetFloat("_rectWave", rectWave);
            material.SetVector("_startPos", startPos);
            Graphics.Blit(source, destination);
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void OnEnable()
    {
        //waveStartTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = new Vector4(Input.mousePosition.x/Screen.width,Input.mousePosition.y/Screen.height,0,0);
            waveStartTime = Time.time;
        }
    }

}
