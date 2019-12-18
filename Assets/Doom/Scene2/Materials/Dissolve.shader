Shader "Custom/Dissolve" {
	Properties {
	    [HDR]
	    _HDRColor("Color", Color) = (1,1,1,1)
	    [HDR]
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DisolveTex ("DisolveTex (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Threshold("Threshold", Range(0,1))= 0.0
		_TimeMag("TimeMag", Range(1,3))= 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		
		#pragma surface surf Standard fullforwardshadows

		
		#pragma target 3.0

		sampler2D _MainTex;
        sampler2D _DisolveTex;
		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _Threshold;
        fixed4 _HDRColor; 
        float _TimeMag;
		
		UNITY_INSTANCING_BUFFER_START(Props)
			
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 m = tex2D(_DisolveTex, IN.uv_MainTex);
            half g = m.r * 0.3 + m.g * 0.7 + m.b * 0.1;
            
            
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			
			_Threshold = abs(sin(_Time.y * _TimeMag));
			
			
			if(g <= _Threshold+0.03){
			    _HDRColor.r = 10.0 + 10.0 * sin(_Time.y*2.0 + IN.uv_MainTex.y);
			    _HDRColor.g = 10.0 + 10.0 * sin(_Time.y*3.0 + IN.uv_MainTex.x);
			    _HDRColor.b = 10.0 + 7.0 * sin(_Time.y*3.0 + IN.uv_MainTex.x + IN.uv_MainTex.y);
			    _HDRColor.a = 1.0;
                o.Albedo = _HDRColor.rgb;
            }
            if(g < _Threshold){
                discard;
            }
		}
		ENDCG
	}
	FallBack "Diffuse"
}
