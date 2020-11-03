Shader "Unlit/functions_distance"
{
    Properties
    {
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

            fixed4 frag (v2f i) : SV_Target
            {
                float r = distance(float2(.5,.5),i.uv)*2;
                //r = sin(r*30);
                //r = abs(r);
                float r2 = length((i.uv-(float2(.5,.5)))*2); 
                //it's the same thing            
                return r;
                //return r2;
            }
            ENDCG
        }
    }
}
