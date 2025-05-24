Shader "Custom/BlackHoleFakeDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _Distortion ("Distortion Strength", Range(0, 0.1)) = 0.03
        _Speed ("Distortion Speed", Range(0, 10)) = 2.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Distortion;
            float _Speed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 distUV : TEXCOORD1;
            };

            float2 DistortUV(float2 uv, float time, float distortion)
            {
                float angle = time + (uv.x + uv.y) * 10.0;
                float2 offset = float2(sin(angle), cos(angle)) * distortion;
                return uv + offset;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.distUV = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 distortedUV = DistortUV(i.uv, _Time.y * _Speed, _Distortion);
                fixed4 tex = tex2D(_MainTex, distortedUV);
                return tex * _Color;
            }
            ENDCG
        }
    }
}