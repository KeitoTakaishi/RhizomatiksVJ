
Shader "Custom/AudioReactiveVertex"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Threshold("Threshold", range(0.0, 1.0)) = 0.75 
	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0
		#include "Noise4d.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 normal;
        };

		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 tangent :TANGENT0;
			float2 texcoord : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
			float2 texcoord2 : TEXCOORD2;
			uint instanceID : SV_InstanceID;
			uint vertexID : SV_VertexID;
		};


		float _Threshold;
		float _Kick;
		void vert(inout appdata v, out Input o) {
			float4 vert = v.vertex;
			//float3 normal = UnityObjectToWorldNormal(v.normal);
			float3 normal = v.normal;

			float scale = 50.0;
			float noise = snoise(float4(vert.x * scale, vert.y * scale, vert.z * scale, _Time.y));
			
			
			if (noise > _Threshold) {
				vert.xyz += normalize(vert.xyz) * noise * _Kick * 0.1;
			}
			
			//vert.xyz += normalize(vert.xyz) * sin(_Time.x);
			v.vertex = vert;

			UNITY_INITIALIZE_OUTPUT(Input, o);
			//o.customColor = abs(v.normal);
			o.uv_MainTex = v.texcoord;
			o.normal = normalize(v.normal);
		}

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 uv = IN.uv_MainTex;
            fixed4 c = tex2D (_MainTex, uv) * _Color;
			//float3 col = IN.normal;
			//c.rgb = col;
			o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
