Shader "Workshop/InstancedMesh"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Scale("Scale", Range(0.01, 1)) = 1
	}

		SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard addshadow

		// set shader target to 5.0 to support structuredbuffer
		#pragma target 5.0

		// enable GPU instancing
		#pragma multi_compile_instancing  
		#pragma instancing_options procedural:setup


		// Include Particle struct
#include "../Shared/Types.hlsl"
#include "../Shared/Matrix.hlsl"

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		float _Scale;
		fixed4 _Color;

		float _MaxLife;
		sampler2D _ColorOverLife;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that	use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			//UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
			UNITY_INSTANCING_BUFFER_END(Props)

			#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
				StructuredBuffer<ParticleBasic> ParticleBuffer;
			#endif


		void setup()
		{

	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

			ParticleBasic p = ParticleBuffer[unity_InstanceID];
			float3 position = p.position;


			float velMag = length(p.velocity);

			float3 scale = 1;

			float4x4 rotateMat = identity_mat();
			float3 angle = 0;
			float3 axis = float3(0, 1, 0);

			//if (length(p.velocity) > 0) {
			//	float3 norm = normalize(p.velocity);
			//	float3 quadNorm = float3(0, 1, 0);
			//	float3 v1 = norm;
			//	float3 v2 = quadNorm;

			//	float3 dotProd = normalize(cross(v2, v1));

			//	scale.y = max(1, 2*smoothstep(0, 1, length(p.velocity)));

			//	//Check for parallel vectors
			//	if (length(dotProd) > 0) {
			//		axis = dotProd;
			//		angle = acos(dot(v2, v1));
			//	}
			//}
	

			float4x4 transform = scale_rotate_translate(_Scale, angle, axis, position);

			unity_ObjectToWorld = transform;

			unity_WorldToObject = inverse(unity_ObjectToWorld);
	#endif
		}


		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			float4 c = _Color;
			o.Albedo = c.rgb;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			o.Alpha = .2;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
