// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/lighting"
{
    Properties
    {
        _LightDirection("Light Direction",vector) = (1,1,1,1)
        _SpecularPower("Specular Power",float) = 1
        _SpecularBrightness("Specular Brightness", float) = 1
        _DiffuseWrap("Wrap",float) = 0
        _DiffuseBrightness("Diffuse Brightness", float ) = 1 
        _RimBrightness("Rim Brightness", float ) = 1
        _RimPower("Rim Power", float ) = 1 
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
                float3 vNormal : COLOR;
                float3 wNormal : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 normal : NORMAL;
                float3 vNormal : COLOR;
                float4 vertex : SV_POSITION;
                float3 wNormal : TANGENT;
                float2 uv : TEXCOORD0;
            };

            float4 _LightDirection;
            float _DiffuseWrap;
            float _SpecularPower;
            float _SpecularBrightness;
            float _DiffuseBrightness;
            float _RimPower;
            float _RimBrightness;
            float3 test;

            float3 reflRay(float3 dir, float3 norm, float3 view, float gamma, float val) {
                
                float3 reflectDir = reflect(-dir, norm);
                float spec = pow(max(dot(view, reflectDir), 0.0), gamma)*val;
                return spec;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal =  mul (unity_ObjectToWorld, v.normal).xyz;
                o.wNormal = normalize(WorldSpaceViewDir(v.vertex));
                o.vNormal = COMPUTE_VIEW_NORMAL;
                o.uv = v.uv;
                return o;
            }           

            fixed4 frag (v2f i) : SV_Target
            {
                float d = dot(normalize(_LightDirection.xyz),i.normal);
                d = max(0,lerp(d,(1+d)*.5,_DiffuseWrap)* _DiffuseBrightness);

                float r = reflRay(normalize(_LightDirection.xyz),i.normal,i.wNormal.xyz,
                _SpecularPower,_SpecularBrightness);

                float b = reflRay(normalize(_LightDirection.xyz),i.normal,-i.wNormal.xyz,
                _SpecularPower*.2,_SpecularBrightness*.2);

                float rim = max(0,pow(((1- i.vNormal.z)*d),_RimPower)*_RimBrightness);
                float rimBack = max(0,pow(((1- i.vNormal.z)*(1-d)),_RimPower*.5)*_RimBrightness*.1);

                return r+d+rim+b+rimBack;
            }
            ENDCG
        }
    }
}
