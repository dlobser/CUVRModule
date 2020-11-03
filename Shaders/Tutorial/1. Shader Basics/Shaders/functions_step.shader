Shader "Unlit/functions_step"
{
    Properties
    {
        _Step("Min",float) = .5
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


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float _Step;

            fixed4 frag (v2f i) : SV_Target
            {
                float r = step(_Step,i.uv.x);
                float s = saturate(sin(6.28*saturate(((i.uv.y-r*.5)-.25)*50)));
                r*=(1-s);
                float4 l = float4(s,0,0,1);
                return r+l;

            }
            ENDCG
        }
    }
}
