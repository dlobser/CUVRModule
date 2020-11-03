Shader "Workshop/Complete/InstancedMesh"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Scale("Scale", Range(0.01, 1)) = 1
		_ScaleInTime("Scale In Time", Range(0,3)) = 0.0
		_ScaleOutTime("Scale Out Time", Range(0,3)) = 0.0
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
		};

		half _Glossiness;
		half _Metallic;
		float _ScaleInTime;
		float _ScaleOutTime;
		float _Scale;
		//fixed4 _Color;

		float _MaxLife;
		sampler2D _ColorOverLife;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that	use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
			UNITY_INSTANCING_BUFFER_END(Props)

			#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
				StructuredBuffer<Particle> ParticleBuffer;
			#endif


		void setup()
		{

	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

			Particle p = ParticleBuffer[unity_InstanceID];
			float3 position = p.position;


			float velMag = length(p.velocity);

			float3 scale = 1;

			float4x4 rotateMat = identity_mat();
			float3 angle = 0;
			float3 axis = float3(0, 1, 0);

			if (length(p.velocity) > 0) {
				float3 norm = normalize(p.velocity);
				float3 quadNorm = float3(0, 1, 0);
				float3 v1 = norm;
				float3 v2 = quadNorm;

				float3 dotProd = normalize(cross(v2, v1));

				scale.y = max(1, 2*smoothstep(0, 1, length(p.velocity)));

				//Check for parallel vectors
				if (length(dotProd) > 0) {
					axis = dotProd;
					angle = acos(dot(v2, v1));
				}
			}
			scale = scale * smoothstep(0, _ScaleInTime, p.age)*(1 - smoothstep(_MaxLife - _ScaleOutTime, _MaxLife, p.age))* float3(.2, min(max(.2, length(p.velocity)), 1), .2) * _Scale;

			float4x4 transform = scale_rotate_translate(scale, angle, axis, position);

			unity_ObjectToWorld = transform;

			_Color = tex2D(_ColorOverLife, p.age / _MaxLife);
			unity_WorldToObject = inverse(unity_ObjectToWorld);
	#endif
		}


		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			float4 c = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
			//float4 c tex2D(_ColorOverLife, i.age / _MaxLife);
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
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
