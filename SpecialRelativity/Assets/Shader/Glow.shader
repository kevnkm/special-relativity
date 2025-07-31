Shader "Custom/Glow"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        [HDR]_GlowColor ("Glow Color (HDR)", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        fixed4 _GlowColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = tex.rgb;
            o.Emission = _GlowColor.rgb; // HDR color will contain intensity
            o.Alpha = tex.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
