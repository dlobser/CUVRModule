#define MODULE_2_4

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class ParticleSolver_Complete : MonoBehaviour
{
    const int NUM_THREADS = 8;

    public int ThreadGroups
    {
        get
        {
            // Need to ceil because we might not be divisible by NUM_THREADS
            // This is an optimization to avoid float div and ceil every frame
            if (_threadGroupsX < 0)
                _threadGroupsX = Mathf.CeilToInt((float)_numParticles / (float)NUM_THREADS);

            return _threadGroupsX;
        }
    }
    int _threadGroupsX = -1;

    public ComputeBuffer ParticleBuffer { get; private set; }

#if MODULE_2_4
    public ComputeBuffer _forceBuffer;

    [SerializeField]
    ExternalForce[] _forces;
#endif

    public int NumParticles { get => _numParticles; }

    public enum Kernel
    {
        Init,
        Update
    }

    public Dictionary<Kernel, int> _kernelDict;

    ComputeBuffer _particleBuffer;

    [SerializeField]
    ComputeShader _particleShader;

    [SerializeField]
    int _numParticles = 100000;

    [SerializeField]
    bool _isInitialized = false;

    [SerializeField, Range(0.5f, 4f)]
    float _sphereSize = 2f;

    [SerializeField]
    public Vector2 _lifeRandom = new Vector2(1, 4);

    [SerializeField]
    Vector3 _constantForce = Vector3.zero;

    [SerializeField]
    Vector3 _noiseForce = Vector3.zero;

    [SerializeField, Range(0.001f, 2f)]
    float _noiseFreq = 1f;

    [SerializeField, Range(0f, 3f)]
    float _noiseSpeed = .5f;

    [SerializeField, Range(0f, 1f)]
    float _dampingFactor = 0f;

    private void CreateKernelDictionary()
    {
        _kernelDict = new Dictionary<Kernel, int>();
        _kernelDict.Add(Kernel.Init, _particleShader.FindKernel("InitParticles"));
        _kernelDict.Add(Kernel.Update, _particleShader.FindKernel("UpdateParticles"));
    }

    private void Awake()
    {
        CreateKernelDictionary();

#if MODULE_2_4
        _forces = FindObjectsOfType<ExternalForce>();
#endif

    }

    void CreateBuffers()
    {
        ParticleBuffer = new ComputeBuffer(_numParticles, ParticleData.stride);

#if MODULE_2_4
        _forceBuffer = new ComputeBuffer(_forces.Length, ExternalForce.ForceData.Stride);
#endif
    }

    void InitSystem()
    {
        DestroyBuffers();
        CreateBuffers();
        BindBuffers();
        UpdateSystemProperties();

        CreateInitialPositions();

        _isInitialized = true;
    }

    private void CreateInitialPositions()
    {
        _particleShader.Dispatch(_kernelDict[Kernel.Init], ThreadGroups, 1, 1);
    }

    private void BindBuffers()
    {
        _particleShader.SetBuffer(_kernelDict[Kernel.Init], "ParticleBuffer", ParticleBuffer);
        _particleShader.SetBuffer(_kernelDict[Kernel.Update], "ParticleBuffer", ParticleBuffer);

#if MODULE_2_4
        _particleShader.SetBuffer(_kernelDict[Kernel.Update], "ForceBuffer", _forceBuffer);
#endif
    }

    private void DestroyBuffers()
    {
        if (ParticleBuffer != null)
            ParticleBuffer.Release();
    }

    private void UpdateSystemProperties()
    {
        if (Application.isEditor)
            BindBuffers();

        _particleShader.SetFloat("_sphereSize", _sphereSize);
        _particleShader.SetVector("_time", new Vector4(Time.time, Time.deltaTime, Time.smoothDeltaTime, Time.unscaledDeltaTime));
        _particleShader.SetFloat("_dampingCoeff", _dampingFactor);
        _particleShader.SetVector("_constantForce", _constantForce);
        _particleShader.SetVector("_noiseForce", _noiseForce);
        _particleShader.SetVector("_lifeRandom", _lifeRandom);
        _particleShader.SetVector("_emitterPos", transform.position);
        _particleShader.SetFloat("_noiseFreq", _noiseFreq);
        _particleShader.SetFloat("_noiseSpeed", _noiseSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isInitialized || Input.GetKeyDown(KeyCode.Space))
            InitSystem();

        CollectForces();

        UpdateSystemProperties();

        _particleShader.Dispatch(_kernelDict[Kernel.Update], ThreadGroups, 1, 1);
    }

    private void CollectForces()
    {
#if MODULE_2_4
        // Only want active forces
        var activeForces = _forces.Where(e => e.gameObject.activeInHierarchy).Select(e => e.Data).ToArray();
        _forceBuffer.SetData(activeForces);
        _particleShader.SetInt("_numForces", activeForces.Length);
#endif

    }
}
