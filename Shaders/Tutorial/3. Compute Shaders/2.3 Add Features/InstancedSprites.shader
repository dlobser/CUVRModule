Shader "Workshop/InstancedPoints"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_Softness("Softness", Range(0,1)) = 0
		_Scale("Scale", Range(0.001,1)) = 0.1
		_ScaleInTime("Scale In Time", Range(0,3)) = 0.0
		_ScaleOutTime("Scale Out Time", Range(0,3)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
		Cull Off
		ZWrite Off
		Blend One One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			// Include Particle struct
			#include "../Shared/Types.hlsl"
			#include "../Shared/Matrix.hlsl"


		

            struct appdata
            {
				uint iid : SV_InstanceID;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			StructuredBuffer<ParticleBasic> ParticleBuffer;
			float _Softness;
			float _Scale;
			float4 _Color;
			float _MaxLife;
			sampler2D _ColorOverLife;
			float _ScaleInTime;
			float _ScaleOutTime;

            v2f vert (appdata v)
            {
				ParticleBasic p = ParticleBuffer[v.iid];
				float3 position = p.position;

				float3 norm = normalize(p.position - _WorldSpaceCameraPos);
				// Normal of Unity built in Quad is +Z (0,0,1)
				float3 quadNorm = float3(0, 0, 1);
				
				float angle = acos(dot(quadNorm, norm));
				float3 axis = normalize(cross(quadNorm, norm));

				float4x4 transform = scale_rotate_translate(_Scale, angle, axis, position);

				v.vertex = mul(transform, v.vertex);
                
				v2f o;
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

                return o;
            }

			float circle(float2 uv)
			{
				half x = length(uv - 0.5) * 2;
				return 1 - smoothstep(1 - _Softness, 1, x);
			}

            fixed4 frag (v2f i) : SV_Target
            {

				float shape = circle(i.uv);

                // sample the texture
				

				fixed4 col = _Color;

				col *= shape*col.a;
                return col;
            }
            ENDCG
        }
    }
}
