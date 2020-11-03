// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/reflection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mix("Mix", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float3 normal : NORMAL;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            //https://docs.unity3d.com/Manual/SL-VertexProgramInputs.html
            struct v2f
            {
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Mix;

            float3 reflRay(float3 dir, float3 norm) {
                float3 ray;
                float ldn = dot(dir, norm);
                float3 refl = 2. * ldn * norm - dir;
                return refl;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float3 forward = mul((float3x3)unity_CameraToWorld, float3(0,0,1));
                o.normal = mul (unity_ObjectToWorld, v.normal).xyz;
                o.normal = reflRay(forward ,o.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 newNormals = float3((i.normal.x+1)*-.25,(i.normal.y+1)*-.5,i.normal.z);
                fixed4 col = tex2D(_MainTex, newNormals.xy);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return lerp(col,float4(-i.normal.xyz,1),saturate(_Mix));
            }
            ENDCG
        }
    }
}
