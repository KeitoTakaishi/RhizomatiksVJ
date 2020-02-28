Shader "Custom/dcgan_gpuparticle"
{
	Properties
	{
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows vertex:vert
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:setup
			#pragma target 3.0
			#include "Assets/ShaderUtils/utils.cginc"
			#include "Assets/ShaderUtils/NoiseUtils.cginc"


			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
				//float2 uv_Tex
				uint id;
				float power;
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
			struct Params {
				float3 pos;
				float2 life;
			};
			StructuredBuffer<Params> paramsBuffer;
	#endif

			void setup() {
	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
	#endif
			}

			float whiteNoise3;
			void vert(inout appdata v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input, o);
	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

				uint _instanceID = (int)v.instanceID;
				int _vid = (int)v.vertexID;

				float4 vert = v.vertex;
				


				float n = 128.0;
				float x = fmod(_instanceID, n);
				float y = (float)_instanceID / n;
				float2 uv = float2(x/n, y/n);				

				//o.uv = uv;
				//o.uv_MainTex = uv;
				//o.uv_Tex = uv;
				o.id = _instanceID;
				o.power = paramsBuffer[_instanceID].life.x;
				
				x = (x * 2.0 - n)/2.0;
				y = (y * 2.0 - n)/2.0;
				
				//scale
				float scale = paramsBuffer[_instanceID].life.x;
				vert.xyz *= scale;
				//TDAbleton
				vert.xyz *= float3(1.0, 1.0, whiteNoise3 * 50.0 * rnd(float2(x, y)));
				
				//vert = mul(MultiRotationMatrix(float3(scale.xxx * 5.0)), vert);
				float3 position = paramsBuffer[_instanceID].pos;
				vert = mul(TranslateMatrix(position), vert);
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
				float n = 128.0;
				float x = fmod((float)IN.id, n);
				float y = (float)IN.id / n;
				float2 uv = float2(x / n, y / n);
				float4 c = tex2D(_MainTex, uv) * _Color * IN.power;
				//float4 c = tex2D(_MainTex, uv) * _Color;
				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
