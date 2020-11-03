using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemBasic : MonoBehaviour
{
    [SerializeField]
    int numParticles = 10000;

    struct Particle{
        public Vector3 Position;
        public Vector3 Velocity;
        public static int stride = 6 * sizeof(float);
    }

    ComputeBuffer particleBuffer;
    public Material material;
    public Mesh mesh;
    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
    ComputeBuffer ArgsBuffer;

    public ComputeShader shader;

    void InitializeParticles()
    {
        List<Particle> particles = new List<Particle>();
        for (int i = 0; i < numParticles; i++)
        {
            Particle particle = new Particle()
            {
                Position = Random.insideUnitSphere,
                Velocity = Vector3.zero
            };
            particles.Add(particle);
        }
        particleBuffer = new ComputeBuffer(numParticles, Particle.stride);
        particleBuffer.SetData(particles);

        ArgsBuffer = new ComputeBuffer(5, 5 * sizeof(uint));
        ArgsBuffer.SetData(args);
    }

  

    void UpdateParticles()
    {
        shader.SetBuffer(0, "ParticleBuffer", particleBuffer);
        shader.SetFloat("_dt", Time.deltaTime);
        shader.Dispatch(0, numParticles / 8, 1, 1);
    }


    void RenderParticles()
    {
        material.SetBuffer("ParticleBuffer", particleBuffer);

        args[0] = mesh.GetIndexCount(0);
        args[1] = (uint)numParticles;
        ArgsBuffer.SetData(args);

        Graphics.DrawMeshInstancedIndirect(
            mesh,
            0,
            material,
            new Bounds(Vector3.zero,Vector3.one*1e6f),
            ArgsBuffer
        );
    }

    void Start()
    {
        InitializeParticles();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            InitializeParticles();

        UpdateParticles();
        RenderParticles();
    }
}
