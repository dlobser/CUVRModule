using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ParticleRenderer_Complete : MonoBehaviour
{
    [SerializeField]
    Mesh _particleMesh;

    [SerializeField]
    Material _mat;

    [SerializeField]
    ParticleSolver_Complete _solver;

    ComputeBuffer _drawArgsBuffer;
    uint[] _drawArgs = new uint[5] { 0, 0, 0, 0, 0 };

    [SerializeField]
    GradientTex _colorOverLife;

    void CreateBuffers()
    {
        DestroyBuffers();

        _drawArgsBuffer = new ComputeBuffer(_drawArgs.Length, sizeof(uint), ComputeBufferType.IndirectArguments);

        _drawArgs[0] = _particleMesh.GetIndexCount(0);
        _drawArgs[1] = (uint)_solver.NumParticles;
        _drawArgs[2] = _particleMesh.GetIndexStart(0);
        _drawArgs[3] = _particleMesh.GetBaseVertex(0);

        _drawArgsBuffer.SetData(_drawArgs);
    }

    private void DestroyBuffers()
    {
        if (_drawArgsBuffer != null)
            _drawArgsBuffer.Release();

        _drawArgsBuffer = null;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void UpdateShaderProperties()
    {
        _mat.SetBuffer("ParticleBuffer", _solver.ParticleBuffer);
        _mat.SetTexture("_ColorOverLife", _colorOverLife.Texture);
        _mat.SetFloat("_MaxLife", _solver._lifeRandom.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (_drawArgsBuffer == null)
            CreateBuffers();

        // wait for particle buffer to be ready
        if (_solver.ParticleBuffer == null)
            return;

        UpdateShaderProperties();

        Graphics.DrawMeshInstancedIndirect(_particleMesh, 0, _mat, new Bounds(Vector3.zero, new Vector3(100, 100, 100)), _drawArgsBuffer, 0, null, ShadowCastingMode.On, true);
    }

    private void OnValidate()
    {
        if (_solver == null)
            _solver = GetComponent<ParticleSolver_Complete>();

        _colorOverLife.BakeTexture();
    }
}
