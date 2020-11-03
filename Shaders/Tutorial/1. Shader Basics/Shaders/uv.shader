Shader "Unlit/uv"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed("Speed",float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex verty
            #pragma fragment frag
           

            #include "UnityCG.cginc"

            struct appdatas
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2fs
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;

            v2fs verty (appdatas v)
            {
                v2fs o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2fs i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
                float3 thing = float3(col.x,col.y,col.z);
                return col;
            }
            ENDCG
        }
    }
}
