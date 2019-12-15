Shader "Custom/BoidsTrailRender"
{
    Properties
    {
		[HDR]
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard  vertex:vert
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:setup
		#pragma target 3.0
		#include "../utils.cginc"

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
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


#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
		StructuredBuffer<float3> positionBuffer;
		StructuredBuffer<float3> velocityBuffer;
#endif

		void setup() {
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
#endif
		}


		int BLOCK_SIZE;
		void vert(inout appdata v) {
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
			uint _instanceID = (int)v.instanceID;
			uint _vid = (int)v.vertexID;

			float4 vert = v.vertex;
			float2 uv = v.texcoord.xy;


			int newVertexID = _vid / 2;

			//bufferアクセス用のindex
			newVertexID = (int)(fmod(floor(_vid / 2), BLOCK_SIZE)) + (_instanceID)* BLOCK_SIZE;

			vert = mul(ScaleMatrix(float3(2.0, 2.0, 2.0)), vert);

			float3 forward = float3(0.0, 0.0, -1.0);
			float theta = dot(forward, positionBuffer[newVertexID]);
			float axis = cross(float3(normalize(positionBuffer[newVertexID])), forward);
			vert = mul(Rodrigues(axis, theta), vert);

			float3 pos = positionBuffer[newVertexID];

			

			vert = mul(TranslateMatrix(pos), vert);
			vert = mul(TranslateMatrix(float3(0.0, 15.0, 0.0)), vert);

			v.vertex = vert;
#endif
		}

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 uv = IN.uv_MainTex;
			uv.y *= 0.7;
			uv.x *= 2.0;
			uv.x = frac(uv.x);
            fixed4 c = tex2D (_MainTex, uv) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
