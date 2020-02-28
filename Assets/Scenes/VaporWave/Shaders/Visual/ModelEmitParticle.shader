
Shader "Custom/ModelEmitParticle"
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

			Cull Off
			LOD 200
			Cull Off ZWrite Off ZTest Always

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows vertex:vert
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:setup
			#pragma target 3.0
			#include "../utils.cginc"
			#include "../Noise4d.cginc"


			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
				float2 id;
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
				//uint vertexID : SV_VertexID;
			};




	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
			StructuredBuffer<float3> positionBuffer;
			StructuredBuffer<float3> lifeBuffer;
	#endif

			void setup() {
	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
	#endif
			}

			int BLOCK_SIZE;
			int res;

			void vert(inout appdata v, out Input i) {
				UNITY_INITIALIZE_OUTPUT(Input, i);
	#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED


				int iID = (int)v.instanceID;
				//uint _vid = (int)v.vertexID;

				float4 vert = v.vertex;
				float2 uv = v.texcoord.xy;
				float3 offSet = positionBuffer[iID];


				float size = lifeBuffer[iID].x * 0.1;
				vert = mul(ScaleMatrix(float3(size, size, size)), vert);

				vert = mul(RotXMatrix(_Time.y + iID + 100.0), vert);
				vert = mul(RotYMatrix(_Time.y + iID + 100.0), vert);
				vert = mul(RotZMatrix(_Time.y + iID + 100.0), vert);

				float3 pos = positionBuffer[iID];
				//float3 pos = float3(0.0, 0.0, iID);
				vert = mul(TranslateMatrix(pos), vert);
				v.vertex = vert;

				i.uv_MainTex = uv;
				//i.id = float2(fmod(iID, 4.0), fmod(iID, 4.0));
	#endif
			}



			fixed4 _Color;
			half _Glossiness;
			half _Metallic;
			float _ScrollSpeed;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				float2 uv = IN.uv_MainTex;
				//float delta = float2(1.0, 1.0) / 4.0;
				//uv /= 4.0;
				//uv += float2(delta * IN.id.x, delta * IN.id.x);
				//uv = uv + float2(_Time.x * _ScrollSpeed, 0.0);
				//uv.x = frac(uv.x);


				fixed4 c = tex2D(_MainTex, uv) * _Color;

				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Albedo = c.rgb;
				o.Alpha = c.a;

			}
			ENDCG
		}
			FallBack "Diffuse"
}

