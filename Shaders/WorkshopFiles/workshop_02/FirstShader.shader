Shader "RLab/FirstShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Data ("Data",vector) = (0,0,0,0)
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
                float3 tangent : TANGENT;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 tang : TANGENT;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float4 _Data;

            v2f vert (appdata v)
            {
                v2f o;
                float timeOffset = _Time.y*_Data.z;
                float waveSelect = min(1,max(0,v.vertex.y*100));
                o.vertex = UnityObjectToClipPos(v.vertex + float3(0, sin(timeOffset+v.vertex.x*6.28*_Data.y)*_Data.w*waveSelect ,0));
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                o.tang = float3(1,1,0);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tint = tex2D(_MainTex, float2(_Data.x,_Data.y));
                fixed4 col = tex2D(_MainTex, i.uv+float2( sin(_Time.y*_Data.z+i.uv.y*6.28*10)*_Data.x,0));
                
                
                return col;
            }
            ENDCG
        }
    }
}
