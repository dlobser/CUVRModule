Shader "Unlit/Sun"
{
    Properties
    {
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
            #include "noise.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 pos : COLOR;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 pos : COLOR;
                float3 normal : NORMAL;
            };

            
            float4 _Data;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.pos = v.vertex;
                float3 zNormal = COMPUTE_VIEW_NORMAL;
                o.pos.w = zNormal.z;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 p = float4(i.pos.x,i.pos.y,i.pos.z,_Time.x);
                float noise0 = abs(snoise(p*4));
                float noise1 = abs(snoise(p*10+noise0));
                float noise1b = abs(snoise(p*20));
                float noise2 = abs(snoise(p*40+noise1b));
                float noise3 = (noise1b+noise1+noise2+noise0)*.1;//(noise1*noise2*noise1b);
                
                float3 red = lerp(float3(1,.3,0),float3(.8,0,0),noise0+.5);
                float3 yellow = float3(1,1,0);
                float3 color = lerp(red,yellow,noise3*5);
                float3 color2 = lerp(float3(1,0,0),color,pow(i.pos.w,2));
                
                color2 += pow(1-i.pos.w,2)*2*float3(1,1,0);
                return color2.xyzx;
            }
            ENDCG
        }
        
        
        Tags { "RenderType"="Transparent" "Queue" = "Transparent+4"}
        LOD 100
        Blend One One
        Cull Back
        ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "noise.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 pos : COLOR;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 pos : COLOR;
                float3 normal : NORMAL;
            };

            
            float4 _Data;

            v2f vert (appdata v)
            {
                v2f o;
                float4 p = float4(v.vertex.x,v.vertex.y,v.vertex.z,_Time.x);

                float noise1 = abs(snoise(p*4));

                o.vertex = UnityObjectToClipPos(v.vertex+v.normal*noise1*.3);
                o.pos = v.vertex;
                float3 zNormal = COMPUTE_VIEW_NORMAL;
                o.pos.w = zNormal.z;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 p = float4(i.pos.x,i.pos.y,i.pos.z,_Time.x);
                float noise1 = abs(snoise(p*4));
                //float noise1b = abs(snoise(p*20));
                //float noise2 = abs(snoise(p*40+noise1b));
                //float noise3 = (noise1b+noise1+noise2+noise0)*.1;//(noise1*noise2*noise1b);
                
                //float3 red = lerp(float3(1,.3,0),float3(.8,0,0),noise0+.5);
                //float3 yellow = float3(1,1,0);
                //float3 color = lerp(red,yellow,noise3*5);
                //float3 color2 = lerp(float3(1,0,0),color,pow(i.pos.w,2));
                
                //color2 += pow(1-i.pos.w,2)*2*float3(1,1,0);
                return max(0,cos(noise1*6.28)*-1)*float4(1,.5,0,1);
            }
            ENDCG
        }
        
        
        Tags { "RenderType"="Transparent""Queue"="Transparent+2" }
        LOD 200
        Blend OneMinusDstColor One
        Cull Front
        //ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal: NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex+v.normal);
                o.uv = v.uv;
                o.normal = COMPUTE_VIEW_NORMAL;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float o = max(0,.5-i.normal.z);
                return pow(o,4)*float4(1,.5,0,0)*.25;
            }
            ENDCG
        }
        
        
    }
}
