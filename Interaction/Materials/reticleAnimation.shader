Shader "Unlit/PolarCoordinates"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Data ("Data",vector) = (1,1,1,1)
        _Percent("Percent",float) = 1
        _Color("Color",color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent""Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off 
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Data;
            float _Percent;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {

                float r_inner=0.0; 
                float t_outer=0.5; 

                float2 x = i.uv - float2(0.5,0.5);
                float radius = length(x);
                float angle = atan2(x.y, x.x);

                float2 tc_polar;
                tc_polar.x = ( radius - r_inner) / (t_outer - r_inner);
                tc_polar.x += _Data.x + _Time.x * _Data.z;
                tc_polar.y = angle * 0.5 / 3.1415 + 0.5;

                float dist = distance(i.uv,float2(.5,.5));
                float c = cos((dist+_Data.y)*6.28);
                c*=_Data.z;

                dist = 1-smoothstep(_Data.x,_Data.x+.001,c);
                float percent = smoothstep(1-_Percent,1-_Percent+.001,tc_polar.y);
                return (1-dist)*(1-percent);
            }
            ENDCG
        }
    }
}
