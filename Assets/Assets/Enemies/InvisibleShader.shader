Shader "Unlit/InvisibleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Light Threshold", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Threshold;

            #ifndef UNITY_LIGHT_ATTENUATION
            #define UNITY_LIGHT_ATTENUATION(i) 1.0
            #endif

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 texColor  = tex2D(_MainTex, i.uv);
                // apply fog
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float lightIntensity = max(0.0, dot(lightDir, float3(0, 0, -1))); // Simple directional light model

                // Set alpha based on light intensity and threshold
                float alpha = lightIntensity < _Threshold ? texColor.a : 0.0;

                return fixed4(texColor.rgb, alpha);
            }
            ENDCG
        }
    }
}
