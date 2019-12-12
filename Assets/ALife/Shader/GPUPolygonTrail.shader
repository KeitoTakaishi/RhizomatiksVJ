// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/GPUPolygonTrail"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			Cull Off
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows vertex:vert
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:setup
			#pragma target 5.0
			#include "utils.cginc"
			#include "NoiseUtils.cginc"
			#include "Noise4d.cginc"

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
			StructuredBuffer<float2> pulseBuffer;
	#endif

			void setup() {
	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
	#endif
			}


			int BLOCK_SIZE;
			uint circleResolution;
			int pulse;
			float trailNum;

			void vert(inout appdata v) {
	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
				uint _instanceID = (int)v.instanceID;
				int _vid = (int)v.vertexID;

				float4 vert = v.vertex;
				float2 uv = v.texcoord.xy;


				//int newVertexID = (int)(fmod((float)(_vid / circleResolution), BLOCK_SIZE)) + (_instanceID)* BLOCK_SIZE;
				int newVertexID = (int)(fmod((float)(_vid / circleResolution), BLOCK_SIZE)) + (_instanceID)* BLOCK_SIZE;

				/*
				if (fmod(_vid, BLOCK_SIZE) != 0.0) {
					float3 dir = positionBuffer[newVertexID - 1] - positionBuffer[newVertexID];
					float theta = atan2(dir.y, dir.z);
					vert = mul(RotXMatrix(theta), vert);
				}
				else {
					float theta = atan2(positionBuffer[newVertexID].y, positionBuffer[newVertexID].z);
					vert = mul(RotXMatrix(theta), vert);
				}
				*/

				float3 pos = positionBuffer[newVertexID];
				vert = mul(TranslateMatrix(pos), vert);

				
				float r = pulseBuffer[newVertexID].y * pulseBuffer[newVertexID].x;
				//float3 anim = normalize(v.vertex);
				//float3 worldNormal = mul((float4x4)unity_ObjectToWorld, v.normal);
				float3 normal = normalize(v.vertex.xyz);
				normal.z = 0.0;
				float3 n = snoise3D(float4(_instanceID, fmod(_vid, circleResolution), 0.0, 0.0));
				normal.xy *= n.xy;
				float3 anim = normalize(normal) * r;
				vert = mul(TranslateMatrix(anim * r), vert);


				//instanceID offSet
				vert = mul(TranslateMatrix(float3( (_instanceID*2.0- trailNum), 0.0, 0.0)), vert);


				v.vertex = vert;
	#endif
			}

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				float2 uv = IN.uv_MainTex;
				fixed4 c = tex2D(_MainTex, uv) * _Color;
				c = _Color;
				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"}
