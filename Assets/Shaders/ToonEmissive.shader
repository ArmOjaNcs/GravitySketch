Shader "Custom/ToonEmissive"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _EmissionColor ("Emission Color", Color) = (0.5,0.5,1,1)
        _RampThreshold ("Light Threshold", Range(0,1)) = 0.5
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Range(0.1, 8)) = 3
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Toon fullforwardshadows
        #pragma target 2.0

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _EmissionColor;
        half _RampThreshold;
        fixed4 _RimColor;
        half _RimPower;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Emission = _EmissionColor.rgb;
            o.Alpha = c.a;
        }

        inline fixed4 LightingToon(SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
        {
            float NdotL = dot(s.Normal, lightDir);
            float toon = step(_RampThreshold, NdotL * atten);

            float rim = 1 - saturate(dot(s.Normal, viewDir));
            rim = pow(rim, _RimPower);

            fixed3 c;
            c = s.Albedo * _LightColor0.rgb * toon + s.Emission + _RimColor.rgb * rim;
            return fixed4(c, 1.0);
        }
        ENDCG
    }
    FallBack "Diffuse"
}