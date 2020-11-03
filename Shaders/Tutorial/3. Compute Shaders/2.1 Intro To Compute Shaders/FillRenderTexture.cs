using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillRenderTexture : MonoBehaviour
{
    /* This is the number of execution threads on the shader
     * can be multi-dimensional
     * Needs to match the arguments in numthreads(x, y, z) in compute shader.
     * 
     * Resolution of texture should be divisible by this number, or you might execute
     * the wrong number of times leading to unexpected results.
     * 
     * Best practice says to make this a multiple of 8, similar for resolution of textures.
    */
    static int NUM_THREADS = 8;

    public int ThreadGroupsX
    {
        get
        {
            return _resolution.x / NUM_THREADS;
        }
    }

    public int ThreadGroupsY
    {
        get
        {
            return _resolution.y / NUM_THREADS;
        }
    }


    [SerializeField]
    ComputeShader _shader;

    [SerializeField]
    Vector2Int _resolution = new Vector2Int(512, 512);

    [SerializeField]
    bool _debugTexture = true;

    [SerializeField, Range(0.1f, 4)]
    float _debugSize = 1f;

    RenderTexture _rt = null;
    public RenderTexture Tex { get => _rt; }

    private void CreateRenderTexture()
    {
        // Create a new render texture
        // Don't need depth or MIPS
        // USe floating point texture because we'll eventually use this for data!
        _rt = new RenderTexture(_resolution.x, _resolution.y, 0, RenderTextureFormat.ARGBFloat);

        _rt.filterMode = FilterMode.Bilinear;

        // Tell the GPU we want to write to this texture
        _rt.enableRandomWrite = true;

        // Make it mirror (good for effects)
        _rt.wrapMode = TextureWrapMode.Mirror;

        // Actually create the RenderTexture on device
        if (!_rt.Create())
        {
            Debug.Log("Failed to create RenderTexture");
            enabled = false;
            return;
        }
    }

    /*
     * 
     * This is where you will Bind intial settings, textures, etc..
     * These should not need to change throughout the execution of
     * your program.
     * 
    */
    private void BindShaderProperties()
    {
        /* When setting a texture/buffer, need to bind to a specific
         * kernel. Since we just have one, use 0 here*/
        _shader.SetTexture(0, "Result", _rt);
        _shader.SetVector("_Resolution", new Vector2(_resolution.x, _resolution.y));
    }

    /*
     * This is where you'll update properties on the shader which 
     * need to change every frame (Time, interactive values, etc...)
     */
    private void UpdateShaderProperties()
    {
        // This is a hack to re-bind textures after shaders get re-compiled
        if (Application.isEditor)
            BindShaderProperties();
    }

    private void init()
    {
        CreateRenderTexture();
        BindShaderProperties();
    }

    void Update()
    {
        if (_rt == null)
            init();

        UpdateShaderProperties();
        _shader.Dispatch(0, ThreadGroupsX, ThreadGroupsY, 1);
    }

    private void OnGUI()
    {
        if (_rt != null && _debugTexture)
            GUI.DrawTexture(new Rect(Vector2.zero, _debugSize * new Vector2(_rt.width, _rt.height)), _rt);
    }
}
