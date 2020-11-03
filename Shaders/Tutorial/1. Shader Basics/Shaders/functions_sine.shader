Shader "Unlit/functions_sine"
{
    Properties
    {
        _Frequency("Frequency",float) = 1
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Frequency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float r = (1+sin(i.uv.x*_Frequency))*.5;
                float s = saturate(sin(6.28*saturate((i.uv.y-r)*10)));
                r*=(1-s);
                float4 l = float4(s,0,0,1);
                return r+l;
            }
            ENDCG
        }
    }
}
