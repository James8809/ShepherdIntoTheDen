Shader "Unlit/ExampleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial) // google what a C buffer is lol
            float4 _BaseColor;  // vec4 basically
        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);   // what is a sampler in HLSL?

        struct VertexInput  // how do HLSL shaders receive per-vertex data?
        {
            float4 position : POSITION; // google what a "semantic is" in hlsl
            float2 uv : TEXCOORD0;  // texture coord 0
        };

        struct VertexOutput
        {
            float4 position : SV_POSITION;  // pixel position
            float2 uv : TEXCOORD0;
        };
        
        ENDHLSL
        
        Pass
        {
            // written in HLSL
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag   // tell HLSL which shaders are which

            VertexOutput vert(VertexInput i)
            {
                VertexOutput o;
                o.position = TransformObjectToHClip(i.position.xyz);
                o.uv = i.uv;
                return o;
            }

            float4 frag(VertexOutput i) : SV_TARGET // semantic ???
            {
                float4 baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                return baseTex * _BaseColor;
            }
            
            ENDHLSL
        }
    }
}
