// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/NoiseUVCloud"
{
    Properties
    {
        _Data ("Data", vector) = (0,0,0,0)

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "noise3D.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPosition : COLOR;

            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPosition : COLOR;

            };

            float4 _Data;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPosition = mul (unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float n1 = abs((snoise(float3(i.uv.x*30,i.uv.y*20,_Time.y))));
                float n2 = abs((snoise(float3(i.uv.x*330*(.9+(n1*.1)),i.uv.y*320*(.9+(n1*.1)),_Time.y))));
                float n0 = abs((snoise(float3(i.uv.x*130,i.uv.y*120,_Time.y+n1))));
                
                float frequency = 10;
                float3 w = i.worldPosition * frequency;
                float n3 = snoise(w);
                float n4 = snoise(float4(i.uv.x*30,i.uv.y*20,0,_Time.y));

                return float4(max(0,n3),max(0,n3),max(0,n3),max(0,n3));
            }
            ENDCG
        }
    }
}
