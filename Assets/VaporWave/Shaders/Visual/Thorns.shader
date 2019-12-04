Shader "Custom/NewSurfaceShader"
{
    Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Magutitude("Magutitude", Range(0.0, 10.0)) = 1.0
		_Threshold("Threshold", Range(0.0, 1.0)) = 1.0
	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0
		#include "../NoiseUtils.cginc"
		#include "../Noise4d.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };



		float coef;
		float _Magutitude;
		float _Threshold;
		int _Range;
		int _Mode;
		void vert(inout appdata_full v) {
			float3 pos = v.vertex.xyz;
			
			if (_Mode == 0) {
				float scale = 100000.0;
				float n = snoise(float4(pos.x * scale, pos.y * scale, pos.z * scale, _Time.x*1.0));

				if (n > _Threshold) {
					v.vertex.xyz += normalize(v.normal.xyz) * n * coef * _Magutitude;
				}
			}
			
			else if (_Mode == 1) {
				float scale = 10.0;
				float n = snoise(float4(pos.x * scale, pos.y * scale, pos.z * scale, _Time.x*1.0));

				if (n > _Threshold) {
					float delta = 1.0 - _Threshold;

					if (n < _Threshold + delta / 2.0) {
						v.vertex.xyz += float3(1.0, 0.0, 0.0)* coef * _Magutitude;
					}
					else {
						v.vertex.xyz += float3(-1.0, 0.0, 0.0)* coef * _Magutitude;
					}
					
				}
			}
			else if (_Mode == 2) {
				float scale = 10.0;
				float n = snoise(float4(pos.x * scale, pos.y * scale, pos.z * scale, _Time.x*1.0));

				if (n > _Threshold) {
					float delta = 1.0 - _Threshold;

					if (n < _Threshold + delta / 2.0) {
						v.vertex.xyz += float3(0.0, 1.0, 0.0)* coef * _Magutitude;
					}
					else {
						v.vertex.xyz += float3(0.0, -1.0, 0.0)* coef * _Magutitude;
					}

				}
			}

			else if (_Mode == 3) {
				float scale = 10.0;
				float n = snoise(float4(pos.x * scale, pos.y * scale, pos.z * scale, _Time.x*1.0));

				if (n > _Threshold) {
					float delta = 1.0 - _Threshold;

					if (n < _Threshold + delta / 2.0) {
						v.vertex.xyz += float3(0.0, 0.0, 1.0)* coef * _Magutitude;
					}
					else {
						v.vertex.xyz += float3(0.0, 0.0, -1.0)* coef * _Magutitude;
					}

				}
			}
			
		}

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
