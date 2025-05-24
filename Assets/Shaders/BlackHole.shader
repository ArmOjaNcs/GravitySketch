Shader "Custom/BlackHole"
{
    Properties
    {
        _HoleColor ("Hole Color", Color) = (0, 0, 0, 1)
        _EdgeSoftness ("Edge Softness", Range(0.01, 1)) = 0.1
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

            fixed4 _HoleColor;
            float _EdgeSoftness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);
                float alpha = smoothstep(0.5, 0.5 - _EdgeSoftness, dist);
                return fixed4(_HoleColor.rgb, alpha);
            }
            ENDCG
        }
    }
}