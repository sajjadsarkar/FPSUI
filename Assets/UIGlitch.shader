Shader "Custom/UIGlitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RGBSplit ("RGB Split", Float) = 0
        _ScanLineJitter ("Scan Line Jitter", Float) = 0
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float _RGBSplit;
            float _ScanLineJitter;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                // Scan line jitter
                uv.y += _ScanLineJitter * sin(uv.y * 100 + _Time.y * 100);
                
                // RGB Split
                fixed4 r = tex2D(_MainTex, uv + float2(_RGBSplit, 0));
                fixed4 g = tex2D(_MainTex, uv);
                fixed4 b = tex2D(_MainTex, uv - float2(_RGBSplit, 0));
                
                return fixed4(r.r, g.g, b.b, g.a) * i.color;
            }
            ENDCG
        }
    }
}
