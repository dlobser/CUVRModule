Shader "Roystan/Toon"
{
    Properties
    {
        _Color("Color",color) = (1,1,1,1)
        _LightColor("Light Color", Color) = (1,1,1,1)
        _DarkColor("Dark Color", Color) = (0.4,0.4,0.4,1)
        _Softness("Softness",float) = .01
         
    }
    SubShader
    {
        Pass
        {
            // Setup our pass to use Forward rendering, and only receive
            // data on the main directional light and ambient light.
            Tags
            {
                "LightMode" = "ForwardBase"
                "PassFlags" = "OnlyDirectional"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Compile multiple versions of this shader depending on lighting settings.
            #pragma multi_compile_fwdbase
            
            #include "UnityCG.cginc"
            // Files below include macros and functions to assist
            // with lighting and shadows.
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;               
                float4 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1; 
                // Macro found in Autolight.cginc. Declares a vector4
                // into the TEXCOORD2 semantic with varying precision 
                // depending on platform target.
                SHADOW_COORDS(2)
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);     
                o.viewDir = WorldSpaceViewDir(v.vertex);
//                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // Defined in Autolight.cginc. Assigns the above shadow coordinate
                // by transforming the vertex from world space to shadow-map space.
                TRANSFER_SHADOW(o)
                return o;
            }
            
            float4 _LightColor;
            float _Softness;

            float4 _DarkColor;
            float4 _Color;

            float4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                // Lighting below is calculated using Blinn-Phong,
                // with values thresholded to creat the "toon" look.
                // https://en.wikipedia.org/wiki/Blinn-Phong_shading_model

                // Calculate illumination from directional light.
                // _WorldSpaceLightPos0 is a vector pointing the OPPOSITE
                // direction of the main directional light.
                float NdotL = dot(_WorldSpaceLightPos0, normal);
                NdotL+=_Softness;
                NdotL/=(2*_Softness);

                // Samples the shadow map, returning a value in the 0...1 range,
                // where 0 is in the shadow, and 1 is not.
                float shadow = SHADOW_ATTENUATION(i);
                // Partition the intensity into light and dark, smoothly interpolated
                // between the two to avoid a jagged break.
                float lightIntensity = smoothstep(0, _Softness, NdotL * shadow); 
                // Multiply by the main directional light's intensity and color.
                float4 light = lerp(_DarkColor,_LightColor,lightIntensity) * _LightColor0;
               
                return light*_Color;
            }
            ENDCG
        }

        // Shadow casting support.
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}