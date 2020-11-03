#ifndef TYPES_HLSL
#define TYPES_HLSL

struct ParticleBasic {
	float3 position;
	float3 velocity;
};

// Must match data EXACTLY to struct in ParticleData.cs
// Vector3 = float3, etc...
struct Particle {
	float3 position;
	float3 velocity;
	float mass;
	float age;
	float life;
};

struct Force {
	float3 position;
	float force;
	float range;
};

#endif