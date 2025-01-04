Shader "Custom/Blink" {
	//https://gist.github.com/thallippoli/4682218
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Blink ( "Blink", Float ) = 0
		_Color("Color", Color) = (1,1,1)
	}
	SubShader {
		Tags {"RenderType"="Transparent" "Queue"="Transparent"}
		Blend SrcAlpha One

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float _Blink;
		float3 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex );
			if( _Blink == 1.0f ) 
				c *=  ( 0.5f + abs( sin( _Time.w ) ) );
			o.Albedo    = c.rgb*_Color;
			o.Alpha     = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
