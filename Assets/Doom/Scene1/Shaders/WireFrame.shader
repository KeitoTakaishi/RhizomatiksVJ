// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/WireFrame"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
		_width("width", Range(0.0, 10.0)) = 1.0
	}
		SubShader
	{
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		Cull Back Zwrite On ZTest LEqual
		LOD 200

		Pass{
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			float4 _Color;
			sampler2D _MainTex;

			struct v2g
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 vertex : TEXCOORD1;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				//float2 uv : TEXCOORD0;
				//float light : TEXCOORD1;
			};

			v2g vert(appdata_full v) {
				v2g o;
				o.vertex = v.vertex;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			float _width;
			[maxvertexcount(18)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
			{
				for (int i = 0; i < 3; i++) {
					//v2gの入力を再定義
					v2g v0 = IN[(i + 0) % 3];
					v2g v1 = IN[(i + 1) % 3];
					v2g v2 = IN[(i + 2) % 3];

					float3 dir = normalize((v1.vertex.xyz + v2.vertex.xyz) * 0.5 - v0.vertex.xyz);

					g2f o;

					o.pos = UnityObjectToClipPos(v1.vertex);
					triStream.Append(o);
					o.pos = UnityObjectToClipPos(v2.vertex);
					triStream.Append(o);
					o.pos = UnityObjectToClipPos(float4( v2.vertex.xyz + dir * _width ,1.0));
					triStream.Append(o);

					triStream.RestartStrip();

					o.pos = UnityObjectToClipPos(v1.vertex);
					triStream.Append(o);
					o.pos = UnityObjectToClipPos(float4(v1.vertex.xyz + dir * _width, 1.0));
					triStream.Append(o);
					o.pos = UnityObjectToClipPos(v2.vertex);
					triStream.Append(o);

					triStream.RestartStrip();
				}




				triStream.RestartStrip();
			}

			half4 frag(g2f i) : COLOR
			{
				//float4 col = tex2D(_MainTex, i.uv);
				//col.rgb *= i.light * _Color;
				float4 col = _Color;
				return col;
			}

			ENDCG
		}
	}
		FallBack "Diffuse"
}
