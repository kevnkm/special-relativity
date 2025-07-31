Shader "Custom/FresnelGlowURP"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 0.3, 1, 1)
        _FresnelPower ("Fresnel Power", Float) = 3.0
        _PulseSpeed ("Pulse Speed", Float) = 2.0
        _GlowIntensity ("Glow Intensity", Float) = 3.0
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        [NoScaleOffset]_NoiseTex ("Noise Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                float3 viewDirWS  : TEXCOORD2;
            };

            float4 _BaseColor;
            float _FresnelPower;
            float _PulseSpeed;
            float _GlowIntensity;
            float _Metallic;
            float _Smoothness;
            sampler2D _NoiseTex;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDirWS = normalize(_WorldSpaceCameraPos - OUT.positionWS);
                OUT.positionCS = TransformWorldToHClip(OUT.positionWS);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float NdotV = saturate(dot(normalize(IN.normalWS), normalize(IN.viewDirWS)));

                // Fresnel terms
                float fresnelOuter = pow(1.0 - NdotV, _FresnelPower);
                float fresnelInner = 1.0 - NdotV;
                float fresnelCombined = lerp(fresnelInner, 1.0, fresnelOuter);

                // Optional animated pulse
                float pulse = 0.5 + 0.5 * sin(_Time.y * _PulseSpeed);
                fresnelCombined *= pulse;

                // Optional panning noise
                float2 uv = IN.positionCS.xy * 0.01 + float2(_Time.y * 0.1, 0.0);
                float noise = tex2D(_NoiseTex, uv).r;
                fresnelCombined *= noise;

                // Final color
                float3 fresnelColor = _BaseColor.rgb * fresnelCombined;
                float alpha = _BaseColor.a * fresnelCombined;

                // Emission and output
                float3 emission = fresnelColor * _GlowIntensity;

                return float4(emission, alpha); // glowing, transparent color
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
