Shader "Custom/Model"
{
    Properties
    {
		[HDR]
		_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Amp("Amp", Range(0, 1)) = 0.0
		
    }
    SubShader
    {
		
        Tags { "RenderType"="Opaque" }
        LOD 200
		Cull Off
        CGPROGRAM
		#include "utils.hlsl"
		#include "noise4D.cginc"
		#pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0	

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		float _Amp;
		float _Audio;
		void vert(inout appdata_full v, out Input o) {
			
			float4 vertex = v.vertex;
			float simplexNoise = snoise(float4(vertex.xyz*10.0, _Time.y * 1.0));
			if (_Audio > 0.5) {
				if (simplexNoise < 0.80) {
					simplexNoise = 0.0;
				}
			}
			else {
				simplexNoise = 0.0;
			}
			float3 n = v.normal * simplexNoise * _Amp * _Audio;
			v.vertex = mul(TranslateMatrix(n), vertex);//translate
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.worldPos = v.texcoord;
			o.worldPos = v.vertex.xyz;
		}


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
			float y = IN.worldPos.y;
			if (frac(y*10.0 +	  _Time.y * 10.0) > 0.75) {
				o.Albedo.rgb *= _Color.rgb;
			}
			else {
				o.Albedo = c.rgb;
			}
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
