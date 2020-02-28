// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Hidden/Dissolve"
{
	Properties{
		[HDR]
		 _Color("Color", Color) = (1,1,1,1)
		[HDR]
		 _EdgeColor("EdgeColor", Color) = (1,1,1,1)
		 _MainTex("Albedo (RGB)", 2D) = "white" {}
		 _DisolveTex("DisolveTex (RGB)", 2D) = "white" {}
		 _Glossiness("Smoothness", Range(0,1)) = 0.5
		 _Metallic("Metallic", Range(0,1)) = 0.0
		 _Threshold("Threshold", Range(0,1)) = 0.0
		 _EdgeThreshold("EdgeThreshold", Range(0,1)) = 0.0

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
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
			half _Threshold;
			half _EdgeThreshold;
			fixed4 _Color;
			fixed4 _EdgeColor;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o) {
				// Albedo comes from a texture tinted by color
				fixed4 m = tex2D(_DisolveTex, IN.uv_MainTex);
				half g = m.r * 0.2 + m.g * 0.7 + m.b * 0.1;
				
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				
				_Threshold = frac(_Time.y);
				half offSet = _EdgeThreshold * 2.0;
				if ( (g > _Threshold - offSet ) && (g < _Threshold - _EdgeThreshold)) {
					c = tex2D(_MainTex, IN.uv_MainTex) * _EdgeColor;
				}
				else if (g < _Threshold) {
					discard;
				}
				
				
				
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		 }
			 FallBack "Diffuse"
}
