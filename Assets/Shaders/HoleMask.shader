Shader "Custom/HoleMaskWithShadows"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _HolePosition ("Hole Position", Vector) = (0,0,0,0)
        _HoleRadius ("Hole Radius", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        fixed4 _Color;
        float4 _HolePosition;
        float _HoleRadius;

        struct Input
        {
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float dist = distance(IN.worldPos.xz, _HolePosition.xz);
            if (dist < _HoleRadius)
            {
                clip(-1); // Аналог discard
            }

            o.Albedo = _Color.rgb;
            o.Alpha = _Color.a;
        }
        ENDCG

        // ShadowCaster pass to ensure shadows work
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _HolePosition;
            float _HoleRadius;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float dist = distance(i.worldPos.xz, _HolePosition.xz);
                if (dist < _HoleRadius)
                {
                    clip(-1); // вырезаем дыру в shadow caster тоже
                }
                return 0;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}