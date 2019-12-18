// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/WireFrame"
{
	Properties
	{
		[HDR]
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_width("width", Range(0.0, 0.1)) = 1.0


		_mode("Mode", Int) = 0.0
		_amp("Amp", Float) = 1.0
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
				float3 normal : TEXCOORD2;
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
				o.normal = v.normal;
				return o;
			}

			float _width;
			float _mode;
			float _amp;
			[maxvertexcount(18)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
			{
				for (int i = 0; i < 3; i++) {
					//v2gの入力を再定義
					v2g v0 = IN[(i + 0) % 3];
					v2g v1 = IN[(i + 1) % 3];
					v2g v2 = IN[(i + 2) % 3];

					float3 n0 = normalize(mul(v0.normal, (float3x3)unity_WorldToObject));
					float3 n1 = normalize(mul(v1.normal, (float3x3)unity_WorldToObject));
					float3 n2 = normalize(mul(v2.normal, (float3x3)unity_WorldToObject));


					//auto
					if (_mode == 0) {
						float amp;
						amp = 10.0 * sin(_Time.y);
						v0.vertex.xyz += amp * n0 * sin(_Time.y + length(v0.vertex));
						v1.vertex.xyz += amp * n1 * sin(_Time.y + length(v1.vertex));
						v2.vertex.xyz += amp * n2 * sin(_Time.y + length(v2.vertex));
					}
					//audio
					else if (_mode == 1) {
						float amp;
						amp = 5.0 * sin(_Time.y + v0.vertex.x  * 10.0 + v1.vertex.y * 10.0 + v2.vertex.z * 10.0);
						v0.vertex.xyz += amp * n0 * sin(_Time.y + length(v0.vertex));
						v1.vertex.xyz += amp * n1 * sin(_Time.y + length(v1.vertex));
						v2.vertex.xyz += amp * n2 * sin(_Time.y + length(v2.vertex));
					}
					else if (_mode == 2) {
						v0.vertex.xyz += 2.5 * _amp * n0 * sin(_Time.y );
						v1.vertex.xyz += 2.5 * _amp * n1 * sin(_Time.y *2.0);
						v2.vertex.xyz += 2.5 * _amp * n2 * sin(_Time.y * 5.0);
					}

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
				float4 col;
			/*
				if (_mode == 0) {
					col = _Color;
				}
				else if(_mode == 1){
					col = _Color;
					col.r = 0.5 + 0.5 * sin(_Time.y);
					col.g = 0.5 + 0.5 * cos(_Time.y * 2.0);
				}
				else {
					col = _Color;
					col.r = 0.5 + 0.5 * sin(_Time.y);
					col.g = 0.5 + 0.5 * cos(_Time.y * 2.0);
				}
				*/
			col = _Color;
				
				return col;
			}

			ENDCG
		}
	}
		FallBack "Diffuse"
}
