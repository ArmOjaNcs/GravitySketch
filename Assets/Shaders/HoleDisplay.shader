﻿Shader "Custom/HoleDisplay"
{
    Properties
    {
        _HoleTex ("Hole Texture", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "Queue"="Geometry+2" }
        ZWrite Off

        Stencil {
            Ref 1
            Comp Equal  // Рендерим только где Stencil = 1
            Pass Keep
        }

        // Черный цвет или текстура
        fixed4 frag() : SV_Target { return fixed4(0,0,0,1); }

        // CGPROGRAM
        // #pragma surface surf Standard

        // sampler2D _HoleTex;

        // struct Input
        // {
        //     float2 uv_HoleTex;
        // };

        // void surf (Input IN, inout SurfaceOutputStandard o)
        // {
        //     fixed4 c = tex2D(_HoleTex, IN.uv_HoleTex);
        //     o.Albedo = c.rgb;
        //     o.Alpha = c.a;
        // }
        // ENDCG
    }
}