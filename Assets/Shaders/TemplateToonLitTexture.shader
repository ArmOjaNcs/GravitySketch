Shader "Custom/ToonLitTexture"
{
     Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _ShadeSteps ("Shade Steps", Range(1,10)) = 4
        _ShadowStrength ("Shadow Strength", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Toon fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        half _ShadeSteps;
        half _ShadowStrength;

        struct Input
        {
            float2 uv_MainTex;
        };

        inline half4 LightingToon(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
        {
            half NdotL = dot(s.Normal, lightDir);
            NdotL = saturate(NdotL * 0.5 + 0.5); // сглаживание переходов

            half toonShade = floor(NdotL * _ShadeSteps) / (_ShadeSteps - 0.5);
            toonShade = lerp(_ShadowStrength, 1.0, toonShade);
            toonShade *= atten;

            half3 c = s.Albedo * _LightColor0.rgb * toonShade;
            return half4(c, s.Alpha);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = tex.rgb;
            o.Alpha = tex.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}