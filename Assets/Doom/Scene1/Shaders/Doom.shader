Shader "Custom/Doom"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		Cull Off ZTest Always

        CGPROGRAM

		#include "SimplexNoise3D.cginc"
		#include "SimplexNoise4D.cginc"
        #pragma surface surf Standard SimpleLambert vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float l;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

		void vert(inout appdata_full v, out Input o) {
			float3 vertex = v.vertex.xyz;
			float3 normal = v.normal.xyz;
			float finness = 10.0;
			float amp = 10.0;

			float noise = simplexNoise(float4(vertex.x*finness, vertex.y*finness, vertex.z*finness, _Time.x*0.3));
			vertex += noise* (-1.0*normal) *amp;
			//vertex.y += frac(vertex.y + _Time.y);

			v.vertex.xyz = vertex.xyz;
			o.l = (v.normal + float3(noise, noise, noise)).x + (v.normal + float3(noise, noise, noise)).y + (v.normal + float3(noise, noise, noise)).z;
			UNITY_INITIALIZE_OUTPUT(Input, o);

		}

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

		half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = dot(s.Normal, lightDir);
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
			c.a = s.Alpha;
			return c;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * IN.l;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
