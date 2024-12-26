Shader "Custom/ShinyUISpriteShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShineColor ("Shine Color", Color) = (1,1,1,1)
        _ShineIntensity ("Shine Intensity", Range(0, 2)) = 1
        _ShineWidth ("Shine Width", Range(0.01, 0.5)) = 0.1
        _ShineSoftness ("Shine Softness", Range(0.01, 0.5)) = 0.1
        _GlowIntensity ("Glow Intensity", Range(0, 2)) = 1
        _ShineSharpness ("Shine Sharpness", Range(0, 10)) = 1
        _Speed ("Shine Speed", Range(-10.0, 10.0)) = 1.0
        _Angle ("Shine Angle", Range(-45.0, 45.0)) = 15.0
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ShineColor;
            float _ShineIntensity;
            float _ShineWidth;
            float _ShineSoftness;
            float _GlowIntensity;
            float _ShineSharpness;
            float _Speed;
            float _Angle;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Create the shining effect
                float2 shineDir = float2(cos(_Angle * UNITY_PI / 180.0), sin(_Angle * UNITY_PI / 180.0));
                float shine = dot(i.uv, shineDir);
                shine = frac(shine + _Time.y * _Speed);
                float edge = smoothstep(0.5 - _ShineWidth - _ShineSoftness, 0.5 - _ShineWidth, shine) - 
                             smoothstep(0.5 + _ShineWidth, 0.5 + _ShineWidth + _ShineSoftness, shine);
                float shineFactor = edge * _ShineIntensity;

                // Apply glow intensity and shine sharpness
                float glowFactor = pow(shineFactor, _ShineSharpness) * _GlowIntensity;

                fixed4 shineColor = _ShineColor * glowFactor;
                shineColor.a = texColor.a * shineFactor; // Ensure shine respects the alpha channel

                fixed4 finalColor = texColor;
                finalColor.rgb += shineColor.rgb;
                finalColor.a = texColor.a;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/VertexLit"
}
