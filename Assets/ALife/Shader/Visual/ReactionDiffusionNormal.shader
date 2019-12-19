﻿Shader "Custom/ReactionDiffusion"
{
	Properties{
  _MainTex("Texture", 2D) = "white" {}
  _BumpMap("Bumpmap", 2D) = "bump" {}
  _Cube("Cubemap", 2D) = "" {}
	}
		SubShader{
		  Tags { "RenderType" = "Opaque" }
		  CGPROGRAM
		  #pragma surface surf Lambert
		  struct Input {
			  float2 uv_MainTex;
			  float2 uv_BumpMap;
			  float3 worldRefl;
			  INTERNAL_DATA
		  };
		  sampler2D _MainTex;
		  sampler2D _BumpMap;
		  samplerCUBE _Cube;
		  void surf(Input IN, inout SurfaceOutput o) {
			  o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * 0.5;
			  o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			  o.Normal = normalize(float3(norm.x, norm.y, 1 - 0.3));
			  o.Emission = texCUBE(_Cube, WorldReflectionVector(IN, o.Normal)).rgb;

			 // float3 norm = UnpackNormal(tex2D(_NormalTex, uv));

		  }
		  ENDCG
	}
		Fallback "Diffuse"
}
