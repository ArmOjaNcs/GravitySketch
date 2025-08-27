Shader "Custom/HoleMaskWithShadows"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}      // текстура (например дерево)
        _Color ("Tint Color", Color) = (1,1,1,1)   // множитель цвета
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

        sampler2D _MainTex;
        fixed4 _Color;
        float4 _HolePosition;
        float _HoleRadius;

        struct Input
        {
            float2 uv_MainTex;   // координаты для текстуры
            float3 worldPos;     // позиция в мире (для дырки)
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // расстояние от текущего пикселя до центра дыры
            float dist = distance(IN.worldPos.xz, _HolePosition.xz);

            // вырезаем дырку
            if (dist < _HoleRadius)
            {
                clip(-1);
            }

            // получаем цвет из текстуры и умножаем на цвет-множитель
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo = tex.rgb;
            o.Alpha  = tex.a;
        }
        ENDCG

        // --- ShadowCaster, чтобы дырка вырезалась и в тенях ---
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
                    clip(-1); // вырезаем дыру в тенях
                }
                return 0;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}