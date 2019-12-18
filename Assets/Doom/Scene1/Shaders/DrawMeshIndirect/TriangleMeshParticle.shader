Shader "Custom/TriangleMeshParticle"
{
    Properties
    {
		[HDR]
        _Color ("Color", Color) = (1,1,1,1)
		_Smoothness("Smoothness", Range(0, 1)) = 0
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
		
        Tags { "RenderType"="Opaque" }
        LOD 200
		Cull Off ZWrite On 
        CGPROGRAM
        #pragma surface surf Standard  vertex:vert addshadow
		#pragma instancing_options procedural:setup

		#if SHADER_TARGET >= 35 && (defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE) || defined(SHADER_API_XBOXONE) || defined(SHADER_API_PSSL) || defined(SHADER_API_SWITCH) || defined(SHADER_API_VULKAN) || (defined(SHADER_API_METAL) && defined(UNITY_COMPILER_HLSLCC)))
			#define SUPPORT_STRUCTUREDBUFFER
		#endif

		#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED) && defined(SUPPORT_STRUCTUREDBUFFER)
			#define ENABLE_INSTANCING
		#endif
		

	
		#if defined(ENABLE_INSTANCING)
		StructuredBuffer<float> LifeBuffer;
		StructuredBuffer<float4> PositionBuffer;
		StructuredBuffer<float4> NormalBuffer;
		#endif
		
		
		half _Glossiness;
		half _Metallic;
		half _Smoothness;
		fixed4 _Color;
        sampler2D _MainTex;

		float4x4 _LocalToWorld;
		float4x4 _WorldToLocal;
		




		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 tangent : TANGENT;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			uint vid : SV_VertexID;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

        struct Input
        {
            float2 uv_MainTex;
        };

        
		void vert(inout appdata v)
		{
			#if defined(ENABLE_INSTANCING)
			
			/*float4x4 transform = float4x4(
				1.0, 0.0, 0.0, 0.0,
				0.0, 1.0, 0.0, 0,
				0.0, 0.0, 1.0, 0.0,
				0.0, 0.0, 0.0, 1.0
				);*/


			uint id = unity_InstanceID * 3 + v.vid;
			v.vertex.xyz = PositionBuffer[id].xyz;
			v.vertex.w = 1.0;
			//v.vertex = mul(transform, v.vertex);

			v.normal = NormalBuffer[id].xyz;
			#endif
		}


		void setup()
		{
			unity_ObjectToWorld = _LocalToWorld;
			unity_WorldToObject = _WorldToLocal;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			o.Albedo = _Color.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			
        }
        ENDCG
    }
    FallBack "Diffuse"
}
