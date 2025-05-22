Shader "Custom/HoleMask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _HolePosition ("Hole Position", Vector) = (0,0,0,0)
        _HoleRadius ("Hole Radius", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            fixed4 _Color;
            float4 _HolePosition;
            float _HoleRadius;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = distance(i.worldPos.xz, _HolePosition.xz);
                if (dist < _HoleRadius)
                {
                    discard; // Прозрачное место
                }
                return _Color;
            }
            ENDCG
        }
    }
}