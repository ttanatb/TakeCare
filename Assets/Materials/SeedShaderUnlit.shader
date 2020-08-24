Shader "Unlit/SeedShaderUnlit"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,0,1,1)
        _Color2 ("Color2", Color) = (1,1,0,1)
        _Color3 ("Color2", Color) = (1,1,0,1)
        _LerpRate ("LerpRate", Vector) = (1,1,0,0)
        _NoiseTex ("Noise", 2D) = "white" {}
        _ShadeTex ("Shade", 2D) = "white" {}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            sampler2D _ShadeTex;

            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;
            fixed4 _LerpRate;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                fixed4 c = lerp(lerp(_Color1, _Color2, abs(sin(_Time.y * _LerpRate.z + i.uv.x * _LerpRate.x ))),
                            _Color3, abs(cos(_Time.x * _LerpRate.w + i.uv.y * _LerpRate.y))) *
                       fixed4(tex2D (_NoiseTex, abs(sin(i.uv + _Time.xy * _LerpRate.wz))).rgb, 1.0) *
                       fixed4(tex2D (_ShadeTex, i.uv).rgb, 1.0);

                // sample the texture
                //fixed4 col = tex2D(_NoiseTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, c);
                return c;
            }
            ENDCG
        }
    }
}
