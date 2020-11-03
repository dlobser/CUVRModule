Shader "Unlit/lighting"
{
    Properties
    {
        _LightDirection("Light Direction",vector) = (1,1,1,1)
        _Wrap("Wrap",float) = 0
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
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            float4 _LightDirection;
            float _Wrap;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float d = dot(normalize(_LightDirection.xyz),i.normal);
                return lerp(d,(1+d)*.5,_Wrap);
            }
            ENDCG
        }
    }
}
