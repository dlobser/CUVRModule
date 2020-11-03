Shader "Class/ClassShader_01"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FlameBottom ("Flame Bottom", color) = (1,1,1,1)
        _FlameTop ("Flame Top", color) = (1,1,1,1)
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
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _FlameBottom;
            float4 _FlameTop;
            
            v2f vert (appdata v)
            {
                v2f o;
                float wave = max(.2,sin(_Time.y));
                o.vertex = UnityObjectToClipPos(v.vertex + float3(sin(_Time.w+v.uv.y*-4)*wave*.2,0,0)*v.uv.y);
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col2 = tex2D(_MainTex, i.uv*5 + float2(0,-_Time.x*.7));

                float4 col = tex2D(_MainTex, i.uv + float2(0,-_Time.y*.1 + col2.r*.1));
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                float stripe = (sin(i.uv.y*10+_Time.z*5)+1)*.5;
                float stripe2 = (sin(i.uv.y*10+_Time.z*5.5435)+1)*.5;
                float4 flameColor = lerp(_FlameBottom,_FlameTop*2,pow(i.uv.y,.5));
                return flameColor+float4((stripe2+.5)*.5,(stripe+.5)*.5,0,0)*.3*col;//float4((sin(_Time.w)+1)*.5,(sin(_Time.z)+1)*.5,(sin(_Time.y)+1)*.5,0);
            }
            ENDCG
        }
    }
}
