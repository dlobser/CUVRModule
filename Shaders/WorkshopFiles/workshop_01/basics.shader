Shader "RLab/basics"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorValues("Color Values", vector) = (0,0,0,0)
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ColorValues;

            v2f vert (appdata v)
            {
                v2f o;
                float3 offset = float3(0,sin(v.vertex.x*10+_Time.y*3)*.2,0);
                o.vertex = UnityObjectToClipPos( v.vertex + offset );
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = float4(offset.y*5,0,0,0);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float uvOffsetX = sin(_Time.w+i.uv.y*6.28*10)*.05;
                float2 weirdUV = i.uv + float2(uvOffsetX,0);
                fixed4 col = tex2D(_MainTex, i.uv );
                float4 strobe = float4((sin(_Time.z*_ColorValues.x)+1)*.5,0,0,0);
                return i.color+col;
            }
            ENDCG
        }
    }
}
