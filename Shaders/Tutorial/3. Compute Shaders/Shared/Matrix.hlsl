#ifndef _MATRXIX_HLSL
#define _MATRIX_HLSL


// Adapted from: https://gist.github.com/keijiro/ee439d5e7388f3aafc5296005c8c3f33
// Rotation with angle (in radians) and axis
float4x4 AngleAxis4x4(float angle, float3 axis)
{
	float c, s;
	sincos(angle, s, c);

	float t = 1 - c;
	float x = axis.x;
	float y = axis.y;
	float z = axis.z;

	return float4x4(
		t * x * x + c, t * x * y - s * z, t * x * z + s * y, 0,
		t * x * y + s * z, t * y * y + c, t * y * z - s * x, 0,
		t * x * z - s * y, t * y * z + s * x, t * z * z + c, 0,
		0, 0, 0, 1
		);
}


float4x4 inverse(float4x4 input)
{
#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
	//determinant(float3x3(input._22_23_23, input._32_33_34, input._42_43_44))

	float4x4 cofactors = float4x4(
		minor(_22_23_24, _32_33_34, _42_43_44),
		-minor(_21_23_24, _31_33_34, _41_43_44),
		minor(_21_22_24, _31_32_34, _41_42_44),
		-minor(_21_22_23, _31_32_33, _41_42_43),

		-minor(_12_13_14, _32_33_34, _42_43_44),
		minor(_11_13_14, _31_33_34, _41_43_44),
		-minor(_11_12_14, _31_32_34, _41_42_44),
		minor(_11_12_13, _31_32_33, _41_42_43),

		minor(_12_13_14, _22_23_24, _42_43_44),
		-minor(_11_13_14, _21_23_24, _41_43_44),
		minor(_11_12_14, _21_22_24, _41_42_44),
		-minor(_11_12_13, _21_22_23, _41_42_43),

		-minor(_12_13_14, _22_23_24, _32_33_34),
		minor(_11_13_14, _21_23_24, _31_33_34),
		-minor(_11_12_14, _21_22_24, _31_32_34),
		minor(_11_12_13, _21_22_23, _31_32_33)
		);
#undef minor
	return transpose(cofactors) / determinant(input);
}

/*
* Taken from https://www.learnopencv.com/rotation-matrix-to-euler-angles/
*/
float4x4 mat_from_euler(float3 euler)
{
	// Calculate rotation about x axis
	float4x4 rx = float4x4(
		1, 0, 0, 0,
		0, cos(euler.x), -sin(euler.x), 0,
		0, sin(euler.x), cos(euler.x), 0,
		0, 0, 0, 1);

	// Calculate rotation about y axis
	float4x4 ry = float4x4(
		cos(euler.y), 0, sin(euler.y), 0,
		0, 1, 0, 0,
		-sin(euler.y), 0, cos(euler.y), 0,
		0, 0, 0, 1
		);

	// Calculate rotation about z axis
	float4x4 rz = float4x4(
		cos(euler.z), -sin(euler.z), 0, 0,
		sin(euler.z), cos(euler.z), 0, 0,
		0, 0, 1, 0,
		0, 0, 0, 1);


	// Combined rotation matrix
	return mul(rz, mul(ry, rx));
}



float4x4 scale_mat(float3 scale) {
	return float4x4(
		scale.x, 0, 0, 0,
		0, scale.y, 0, 0,
		0, 0, scale.z, 0,
		0, 0, 0, 1
		);
}

float4x4 translate_mat(float3 translate)
{
	return float4x4(1, 0, 0, translate.x,
		0, 1, 0, translate.y,
		0, 0, 1, translate.z,
		0, 0, 0, 1);

}

float4x4 identity_mat()
{
	return float4x4(1, 0, 0, 0,
		0, 1, 0, 0,
		0, 0, 1, 0,
		0, 0, 0, 1);
}


float4x4 scale_rotate_translate(float3 scale, float3 angle, float3 axis, float3 translate)
{
	return mul(mul(translate_mat(translate), AngleAxis4x4(angle, axis)), scale_mat(scale));
}


#endif