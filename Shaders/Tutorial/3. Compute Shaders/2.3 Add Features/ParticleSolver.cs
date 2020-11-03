#define MODULE_2_4

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class ParticleSolver : MonoBehaviour
{
    #region Compute Types
    public struct ParticleData
    {
        public Vector3 Position;
        public Vector3 Velocity;

        public static int stride = 6 * sizeof(float);
    }
    #endregion

    const int NUM_THREADS = 128;

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

    public static int UpdateKernel = 0;

    public ComputeBuffer ParticleBuffer { get; private set; }


    public int NumParticles { get => _numParticles; }

    [SerializeField]
    ComputeShader _shader;

    [SerializeField]
    int _numParticles = 10000;

    [SerializeField]
    bool _isInitialized = false;

    [SerializeField]
    ExternalForce _force;

    [SerializeField, Range(0,1)]
    float _dampingCoefficient = 0.1f;

    private void Awake()
    {

    }

    void CreateBuffers()
    {
        ParticleBuffer = new ComputeBuffer(_numParticles, ParticleData.stride);
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
        List<ParticleData> particles = new List<ParticleData>(_numParticles);

        for (int i = 0; i < _numParticles; i++)
        {
            particles.Add(new ParticleData()
            {
                Position = Random.insideUnitSphere,
                Velocity = Vector3.zero
            });
        }

        ParticleBuffer.SetData(particles);
    }

    private void BindBuffers()
    {
        _shader.SetBuffer(0, "ParticleBuffer", ParticleBuffer);
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

        _shader.SetVector("_forcePosition", _force.Data.Position);
        _shader.SetFloat("_forceStrength", _force.Data.Force);
        _shader.SetVector("_time", new Vector4(Time.time, Time.deltaTime, Time.smoothDeltaTime, 0));
        _shader.SetFloat("_dampingCoeff", _dampingCoefficient);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isInitialized || Input.GetKeyDown(KeyCode.Space))
            InitSystem();

        UpdateSystemProperties();

        _shader.Dispatch(UpdateKernel, ThreadGroups, 1, 1);
    }
}
