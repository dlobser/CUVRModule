Shader "Unlit/water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Offset("Offset",float) = 1
        _Frequency("Frequency",float) = 1
        _Speed("Speed",float) = 1
        _NormalOffset("Normal Offset", float) = 1
        _ColorA("Color A", color)=(1,1,1,1)
        _ColorB("Color B", color)=(1,1,1,1)
        _Power("Power",float) = 1
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
            #include "Noise.cginc"
            struct appdata
            {
                float3 normal : NORMAL;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPosition : COLOR;

            };

            struct v2f
            {
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPosition : COLOR;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Offset;
            float _Frequency;
            float _NormalOffset;
            float _Speed;
            float _Power;
            float4 _ColorA;
            float4 _ColorB;

            v2f vert (appdata v)
            {
                v2f o;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPosition = mul (unity_ObjectToWorld, v.vertex).xyz;
                float noise = snoise(o.worldPosition*_Frequency+_Time.z*_Speed);
                //noise = pow(noise,_Power);
                o.vertex = UnityObjectToClipPos(v.vertex + v.normal * _NormalOffset * noise);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 newUV = float2(i.uv.x+_Time.x*_Speed,i.uv.y);
                float noise = snoise(i.worldPosition*_Frequency+_Time.z*_Speed);
                noise = pow((noise+1)*.5,_Power);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                float4 outColor = lerp(_ColorA,_ColorB,noise);
                return outColor;//float4(newUV,1,1);
            }
            ENDCG
        }
    }
}
