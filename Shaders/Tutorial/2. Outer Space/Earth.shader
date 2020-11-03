Shader "Unlit/Earth"
{
    Properties
    {
        _Data ("Data", vector) = (0,0,0,0)
        _Light("Light",vector) = (0,1,0)
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
                float3 normal : NORMAL;
                float3 tangent : TANGENT;
                float2 uv : TEXCOORD0;
                float4 pos : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float3 tangent : TANGENT;
                float4 vertex : SV_POSITION;
                float4 pos : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Data;
            float3 _Light;
            
            //calculate the gradient of noise to modify the normals
            
            float3 grad(float3 p,float o){
                float r = snoise(p);
                return float3(
                    (snoise(p+float3(o,0,0))-r)/o,
                    (snoise(p+float3(0,o,0))-r)/o,
                    (snoise(p+float3(0,0,o))-r)/o
                    );
            }

            v2f vert (appdata v)
            {
                v2f o;
                
                //the noise we're using to offset the vertices to make mountains
                float n = pow(max(0,snoise(v.vertex*_Data.x)-_Data.y),2);
  
                o.normal = v.normal;
                
                //use the tangent channel pass world space normals to the frag shader  
                o.tangent = mul (unity_ObjectToWorld, v.normal).xyz;
                
                //offset the vertices with the noise
                o.vertex = UnityObjectToClipPos(v.vertex+v.normal*n*_Data.z);
                
                o.pos = v.vertex;
                
                //Sneak the view normals into the world position
                float3 zNormal = COMPUTE_VIEW_NORMAL;
                o.pos.w = zNormal.z;               

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                //calculate different layers of noise with different frequencies
                float x = snoise(i.pos * _Data.x )-_Data.y;
                float x2 = snoise(i.pos * _Data.x * 22 )-_Data.y;
                float x3 = snoise(i.pos * _Data.x * 8 + _Time.y )-_Data.y;
                float x4 = snoise(100 + i.pos * _Data.x );
                
                //create 2 layers of grad noise to offset the normals
                float3 noise = normalize(grad(i.pos*_Data.x,.001)*.2)*min(1,_Data.z*2);
                noise = mul (unity_ObjectToWorld,noise);
                float3 noise2 = normalize(grad(i.pos*42,.001))*.051;
                noise2 = mul (unity_ObjectToWorld,noise2);
                
                //combine noises and interpolate between the base sphere normals
                float3 on = lerp(
                i.tangent+noise2*.1,
                normalize(i.tangent-noise+lerp(noise2,float3(0,0,0),smoothstep(1.5,1.6,x+x2*.05+1))),
                smoothstep(_Data.y,_Data.y+.1,x)
                );
                
                float lighting = dot(on,normalize(_Light));
                
                //create layers of color using the original noise
                float3 water = lerp(float3(0,.5,1),float3(0,.4,.9),x3+1);
                float3 land = lerp(float3(.3,1,.1),float3(.8,.9,0),(x4+1)*.5);
                float3 snow = float3 (1,1,1);
                float3 rim = float3(.5,.8,1);
                
                float3 waterLand = lerp(water,land,smoothstep(.45,.51,x+x3*.05+1));
                float3 waterLandSnow = lerp(waterLand,snow,smoothstep(1.5,1.6,x+x2*.05+1));
                float3 withRim = lerp(waterLandSnow,rim,pow(1-i.pos.w,2)*2);
                float3 oRim = pow((1-i.pos.w),3)*rim;
                
                return pow((max(0,min(1,lighting+.05))),1.5) * withRim.xyzx + oRim.xyzx;
            }
            ENDCG
        }
        
        //Create a glowing atmosphere by inverting the sphere and using camera normal z
        Tags { "RenderType"="Transparent""Queue"="Transparent" }
        LOD 200
        Blend OneMinusDstColor One
        Cull Front
        ZWrite Off
        
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
                return pow(o,4)*float4(0,.5,1,0)*.5;
            }
            ENDCG
        }
        
        //two layers of clouds
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 pos : COLOR;
                float3 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                float4 pos : COLOR;
                float3 tangent : TANGENT;

            };

            float4 _Data;
            float3 _Light;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex+v.normal*.05);
                o.pos = v.vertex;
                float3 zNormal = COMPUTE_VIEW_NORMAL;
                o.pos.w = zNormal.z; 
                o.tangent = mul (unity_ObjectToWorld, v.normal).xyz;
              
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //use the same values for noise to avoid having clouds on mountains
                float x = snoise(i.pos * _Data.x )-_Data.y+-.5;
                //add some detail to the noise
                x *= max(0,snoise(_Time.x+i.pos * _Data.x*3 )-_Data.y+snoise(_Time.x+i.pos * _Data.x*.5 )-.5);
                
                //clip removes pixels that are lower than 0
                clip((x*-1)-.05);
                //create lighting
                float l = dot(i.tangent,normalize (_Light))+.3;

                return float4(l*.6,l*.7,l,.8);
            }
            ENDCG
        }
        
        //above comments relate to this layer of clouds, slight tweaks to color and clip level
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            //#include "noise.cginc"
            #include "noise.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 pos : COLOR;
                float3 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                float4 pos : COLOR;
                float3 tangent : TANGENT;

            };

            float4 _Data;
            float3 _Light;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex+v.normal*.065);
                o.pos = v.vertex;
                float3 zNormal = COMPUTE_VIEW_NORMAL;
                o.pos.w = zNormal.z; 
                o.tangent = mul (unity_ObjectToWorld, v.normal).xyz;
              
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float x = snoise(i.pos * _Data.x )-_Data.y-.5;
                x *= max(0,snoise(_Time.x+i.pos * _Data.x*3 )-_Data.y+snoise(_Time.x+i.pos * _Data.x*.5 )-.5);

                clip((x*-1)-.1);
                float l = dot(i.tangent,normalize (_Light))+.4;

                return float4(l,l,l,1);
            }
            ENDCG
        }
        
    }
    
}
