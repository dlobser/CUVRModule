using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCubes : MonoBehaviour
{
    const int NUM_THREADS = 8;
    public int ThreadGroupsX { get { return _planeDims.x / NUM_THREADS; } }
    public int ThreadGroupsY { get { return _planeDims.y / NUM_THREADS; } }

    [SerializeField]
    FillRenderTexture _offsetTexture;

    ComputeBuffer _positionsBuffer;
    //ComputeBuffer _rotationsBuffer;
    //ComputeBuffer _scalesBuffer;
    //ComputeBuffer _colorBuffer;

    /* Need this buffer for indirect rendering
     * will hold an array for this buffer for performance reasons
     * structure of this buffer
     * args[0] - number of indices per mesh
     * args[1] - number of instances to draw of this mesh
     * args[2] - offset into index buffer to start (usually zero)
     * args[3] - base vertex location (usually zero)
     * args[4] - instance start offset (usually zero)
    */
    ComputeBuffer _drawArgsBuffer;
    private uint[] _args = new uint[5] { 0, 0, 0, 0, 0 };

    [SerializeField]
    Mesh _cubeMesh;

    [SerializeField]
    Material _renderMaterial;

    [SerializeField]
    int _numCubes;

    [SerializeField]
    Vector2Int _planeDims = new Vector2Int(10, 10);

    //[SerializeField]
    //ComputeShader _shader;

    [SerializeField]
    bool _isInitialized = false;

    // Create buffers on the GPU
    void CreateBuffers()
    {
        /* 
         * new ComputeBuffer(int count, int stride)
         * count is the number of elements in the buffer
         * stride is the size(in bytes) per element in the buffer
         * 
         * for _argsBuffer, need to specify ComputeBufferType.IndirectArguments
         * to tell the GPU this buffer is used for a draw command
        */
        _positionsBuffer = new ComputeBuffer(_numCubes, 3 * sizeof(float));
        //_rotationsBuffer = new ComputeBuffer(_numCubes, 3 * sizeof(float));
        //_colorBuffer = new ComputeBuffer(_numCubes, 4 * sizeof(float));
        //_scalesBuffer = new ComputeBuffer(_numCubes, 3 * sizeof(float));

        _drawArgsBuffer = new ComputeBuffer(1, _args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);

        // Setup args buffer for rendering
        _args[0] = (uint)_cubeMesh.GetIndexCount(0);
        _args[1] = (uint)_numCubes;
        _args[2] = (uint)_cubeMesh.GetIndexStart(0);
        _args[3] = (uint)_cubeMesh.GetBaseVertex(0);
        _drawArgsBuffer.SetData(_args);
    }

    //// Initialize shader props
    //private void BindShaderProperties()
    //{
    //    // Bind Buffers. Again, need to specify kernel index
    //    _shader.SetBuffer(0, "_PositionsBuffer", _positionsBuffer);
    //    //_shader.SetBuffer(0, "_RotationsBuffer", _rotationsBuffer);
    //    //_shader.SetBuffer(0, "_ColorBuffer", _colorBuffer);
    //    //_shader.SetBuffer(0, "_ScalesBuffer", _scalesBuffer);

    //    // Bind our texture that we are filling in the other shader
    //    _shader.SetTexture(0, "_InputTex", _offsetTexture.Tex);
    //    _shader.SetFloats("_InputTexResolution",
    //        new float[] { _offsetTexture.Tex.width, _offsetTexture.Tex.height });

    //    // Provide plane dimensions for calculating index
    //    _shader.SetFloats("_PlaneDims", new float[] { _planeDims.x, _planeDims.y });
    //}

    //// Update shader props
    //private void UpdateShaderProperties()
    //{
    //    // This is a hack to re-bind textures after shaders get re-compiled (not needed in play mode)
    //    if (Application.isEditor)
    //        BindShaderProperties();
    //}

    void UpdateMaterialProperties()
    {
        _renderMaterial.SetBuffer("PositionsBuffer", _positionsBuffer);
        //_renderMaterial.SetBuffer("RotationsBuffer", _rotationsBuffer);
        //_renderMaterial.SetBuffer("ColorBuffer", _colorBuffer);
        //_renderMaterial.SetBuffer("ScalesBuffer", _scalesBuffer);

        _renderMaterial.SetTexture("_OffsetTex", _offsetTexture.Tex);
        _renderMaterial.SetVector("_planeDims", new Vector4(_planeDims.x, _planeDims.y, 0, 0));
    }


    private void SetInitialValues()
    {
        List<Vector3> positions = new List<Vector3>(_numCubes);
        //List<Vector3> rotations = new List<Vector3>(_numCubes);
        //List<Vector3> scales = new List<Vector3>(_numCubes);

        // Create a grid of positions
        Vector3 _startPos = new Vector3(-_planeDims.x / 2f, .5f, -_planeDims.y / 2f);

        for (int x = 0; x < _planeDims.x; x++)
            for (int z = 0; z < _planeDims.y; z++)
            {
                positions.Add(_startPos + new Vector3(x, 0, z));
                //Debug.Log(positions[positions.Count-1]);
                //rotations.Add(Vector3.zero);
                //scales.Add(Vector3.one);
            }

        _positionsBuffer.SetData(positions);
        //_rotationsBuffer.SetData(rotations);
        //_scalesBuffer.SetData(scales);
    }

    private void Init()
    {
        CreateBuffers();
        SetInitialValues();
        //BindShaderProperties();

        _isInitialized = true;
    }


    // Update is called once per frame
    void Update()
    {
        // Wait for our RenderTexture to be available
        if ((!_isInitialized && _offsetTexture.Tex != null) || Input.GetKeyDown(KeyCode.Space))
            Init();

        if (!_isInitialized)
            return;

        //UpdateShaderProperties();

        // Dispatch the compute shader
        //_shader.Dispatch(0, ThreadGroupsX, ThreadGroupsY, 1);

        // Update material props
        UpdateMaterialProperties();


        // Actually render the cubes
        Graphics.DrawMeshInstancedIndirect(_cubeMesh, 0, _renderMaterial, new Bounds(Vector3.zero, new Vector3(100, 100, 100)), _drawArgsBuffer, 0);
    }


    /*
     * Good practice to dispose of your GPU buffers 
     * when you're done with them.
     * 
     * Also, Unity will complain if you don't!
     * 
     */
    void DisposeBuffers()
    {
        if (_positionsBuffer != null)
        {
            _positionsBuffer.Dispose();
            _positionsBuffer = null;
        }

        //if (_rotationsBuffer != null)
        //{
        //    _rotationsBuffer.Dispose();
        //    _rotationsBuffer = null;
        //}

        if (_drawArgsBuffer != null)
        {
            _drawArgsBuffer.Dispose();
            _drawArgsBuffer = null;
        }
    }

    private void OnDestroy()
    {
        DisposeBuffers();
    }

    private void OnValidate()
    {
        _numCubes = _planeDims.x * _planeDims.y;
        _offsetTexture = GetComponent<FillRenderTexture>();
    }

}
