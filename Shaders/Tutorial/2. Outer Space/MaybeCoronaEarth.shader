Shader "Unlit/MaybeCoronaEarth"
{
    Properties
    {
        _Data1("Data1",vector) = (0,0,0,0)
        _Data2("Data2",vector) = (0,0,0,0)
        _LightDir("Light",vector) = (0,0,0,0)
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 wPos : COLOR;
                float3 tang : TANGENT;
                float3 normal : NORMAL;

            };
            
            float4 _Data1;
            float4 _Data2;
            float4 _LightDir;
            
            float3 grad(float3 p){
                float o = .001;
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
                
                o.wPos = mul (unity_ObjectToWorld, v.vertex).xyz;
                
                float multiply = _Data1.y;
                float frequency = _Data1.x;
                
                float n = snoise(o.wPos*frequency)*multiply;
                n = max(0,n);
                o.vertex = UnityObjectToClipPos(v.vertex * (1+n));

                o.tang = v.normal;
                o.normal = v.normal;
                
                return o;
            }
            
           

            fixed4 frag (v2f i) : SV_Target
            {
                float frequency = _Data1.x;
                float colorNoise = snoise(i.wPos*frequency);
                float colorNoise2 = snoise(i.wPos*_Data1.z);

                float3 noise = normalize(grad(i.wPos*frequency))*.5;
                float3 modifiedNormal = normalize(i.normal-noise);
                
                float3 newNormal = lerp(i.normal,modifiedNormal,clamp(colorNoise*10,0,1));
                
                float lighting = dot(normalize(_LightDir.xyz),newNormal);
                
                
                float4 water = float4(0,.5,1,1);
                float4 forest = float4(.7,1,0,1);
                float4 snow = float4(1,1,1,1);
                
                float colorLerp = (((colorNoise+colorNoise2*.05)*.95)+1)*.5;
                
                float4 waterToForest = lerp(water, forest,smoothstep(.15,.17,colorLerp));
                float4 forestToSnow = lerp(waterToForest,snow,smoothstep(.85,.86,colorLerp));
                
                return lighting*forestToSnow;
            }
            ENDCG
        }
        
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 wPos : COLOR;
                float3 tang : TANGENT;
                float3 normal : NORMAL;

            };
            
            float4 _Data1;
            float4 _Data2;
            float4 _LightDir;
           
            
            v2f vert (appdata v)
            {
                v2f o;
                
                o.wPos = mul (unity_ObjectToWorld, v.vertex).xyz;
                
                float multiply = _Data1.y;
                float frequency = _Data1.w;
                
                float n = snoise(o.wPos*frequency)*_Data2.x;
                n = max(0,n);
                o.vertex = UnityObjectToClipPos(v.vertex * (1+n) + v.normal * _Data2.z);

                o.tang = v.normal;
                o.normal = v.normal;
                
                return o;
            }
            
           

            fixed4 frag (v2f i) : SV_Target
            {
                float frequency = _Data1.w;
                float colorNoise = snoise(i.wPos*frequency);
                float colorNoise2 = snoise(i.wPos*_Data1.z);

                //float3 noise = normalize(grad(i.wPos*frequency))*.5;
                //float3 modifiedNormal = normalize(i.normal-noise);
                
                //float3 newNormal = lerp(i.normal,modifiedNormal,clamp(colorNoise*10,0,1));
                
                float lighting = max(0,dot(normalize(_LightDir.xyz),i.normal));
               
                
                return float4(lighting,lighting,lighting,max(0,smoothstep(_Data2.y,_Data2.y+.2,colorNoise)));//lighting;
            }
            ENDCG
        }
        
        
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend One One
        LOD 100
        Cull Front
        
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 wPos : COLOR;
                float3 tang : TANGENT;
                float3 normal : NORMAL;

            };
            
            float4 _Data1;
            float4 _Data2;
            float4 _LightDir;
           
            
            v2f vert (appdata v)
            {
                v2f o;
                
                //o.wPos = mul (unity_ObjectToWorld, v.vertex).xyz;
                
                //float multiply = _Data1.y;
                //float frequency = _Data1.w;
                
                //float n = snoise(o.wPos*frequency)*_Data2.x;
                //n = max(0,n);
                o.vertex = UnityObjectToClipPos(v.vertex + v.normal * _Data2.w);

                //o.tang = v.normal;
                o.normal = COMPUTE_VIEW_NORMAL;
                
                return o;
            }
            
           

            fixed4 frag (v2f i) : SV_Target
            {
                //float frequency = _Data1.w;
                //float colorNoise = snoise(i.wPos*frequency);
                //float colorNoise2 = snoise(i.wPos*_Data1.z);

                ////float3 noise = normalize(grad(i.wPos*frequency))*.5;
                ////float3 modifiedNormal = normalize(i.normal-noise);
                
                ////float3 newNormal = lerp(i.normal,modifiedNormal,clamp(colorNoise*10,0,1));
                
                //float lighting = max(0,dot(normalize(_LightDir.xyz),i.normal));
               
                
                return pow((1-i.normal.z),5)*.1* float4(0,.7,1,1);
            }
            ENDCG
        }
        
    }
}
