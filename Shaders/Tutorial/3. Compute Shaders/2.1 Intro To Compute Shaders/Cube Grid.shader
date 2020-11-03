Shader "Workshop/CubeGrid"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM

			#pragma surface surf Standard addshadow

			// set shader target to 5.0 to support structuredbuffer
			#pragma target 5.0

			// enable GPU instancing
			#pragma multi_compile_instancing  
			#pragma instancing_options procedural:setup

			sampler2D _MainTex;

		sampler2D _OffsetTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			float2 _planeDims;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
				//UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
			UNITY_INSTANCING_BUFFER_END(Props)

	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
				StructuredBuffer<float3> PositionsBuffer;
			//StructuredBuffer<float3> RotationsBuffer;
			//StructuredBuffer<float3> ScalesBuffer;
			//StructuredBuffer<float4> ColorBuffer;
	#endif

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

			void setup()
			{
	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
				
				// Access the buffers we've created and filled
				float3 position = PositionsBuffer[unity_InstanceID];
				
				// Calculate "UV" coord based on cube XZ position
				float2 uv = position.xz / _planeDims;
				uv += float2(.5, .5);

				float4 offset = tex2Dlod(_OffsetTex, float4(uv, 0, 0));

				float3 scale = float3(.8, offset.x, .8);

				float4x4 scaleMat = {
					scale.x,0,0,0,
					0,scale.y,0,0,
					0,0,scale.z,0,
					0,0,0,1
				};

				float4x4 translateMat = {
					1, 0, 0, position.x,
					0, 1, 0, position.y,
					0, 0, 1, position.z,
					0, 0, 0, 1
				};




				float c = cos(offset.y);
				float s = sin(offset.y);
				float4x4 rotateMat = float4x4(
					c, 0, s, 0,
					0, 1, 0, 0,
					-s, 0, c, 0,
					0, 0, 0, 1);

				float4x4 transform = mul(translateMat, rotateMat);
				transform = mul(transform, scaleMat);

				unity_ObjectToWorld = transform;


				unity_WorldToObject = inverse(unity_ObjectToWorld);
	#endif
			}


			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				//float4 c = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
				float4 c = _Color;
				//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
