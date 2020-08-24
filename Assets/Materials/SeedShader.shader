Shader "Custom/SeedShader"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,0,1,1)
        _Color2 ("Color2", Color) = (1,1,0,1)
        _Color3 ("Color3", Color) = (1,1,0,1)
        _LerpRate ("LerpRate", Vector) = (1,1,0,0)
        _NoiseTex ("Noise", 2D) = "white" {}
        // _MainTex ("Albedo (RGB)", 2D) = "white" {}
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0


        struct Input
        {
            float2 uv_MainTex;
        };

        // half _Glossiness;
        // half _Metallic;
        sampler2D _NoiseTex;
        fixed4 _Color1;
        fixed4 _Color2;
        fixed4 _Color3;
        fixed4 _LerpRate;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = lerp(lerp(_Color1, _Color2, abs(sin(_Time.y * _LerpRate.z + IN.uv_MainTex.x * _LerpRate.x ))),
                            _Color3, abs(cos(_Time.x * _LerpRate.w + IN.uv_MainTex.y * _LerpRate.y))) *
                       fixed4(tex2D (_NoiseTex, abs(sin(IN.uv_MainTex + _Time.xy * _LerpRate.wz))).rgb, 1.0);

            o.Albedo = c.rgb;

            // Metallic and smoothness come from slider variables
            o.Metallic = 0;
            o.Smoothness = 0;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
