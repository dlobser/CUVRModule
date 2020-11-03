// Pseudo random number generator (prng) taken from
// https://github.com/keijiro/ComputePrngTest
// Hash function from H. Schechter & R. Bridson, goo.gl/RXiKaH
uint Hash(uint s)
{
	s ^= 2747636419u;
	s *= 2654435769u;
	s ^= s >> 16;
	s *= 2654435769u;
	s ^= s >> 16;
	s *= 2654435769u;
	return s;
}

float Random(uint seed)
{
	return float(Hash(seed)) / 4294967295.0; // 2^32-1
}

// More or less from here: 
// https://www.gamedev.net/forums/topic/652701-generating-random-points-in-a-sphere/
float3 random_inside_sphere(uint id)
{
	float u1 = 2 * (Random(id) - .5);
	float u2 = Random(id - 2346);
	float r = sqrt(1.0f - u1 * u1);
	float theta = 2.0f*3.1416*u2;

	return Random(id + 901)*float3(r*cos(theta), r*sin(theta), u1);
}