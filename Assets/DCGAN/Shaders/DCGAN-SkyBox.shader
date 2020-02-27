/*
Shader "Unlit/DCGAN-SkyBox"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Background"
			"Queue" = "Background"
			"PreviewType" = "SkyBox"
		}

		Pass
		{
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float3 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			sampler2D _MainTex;
			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
				//uv.y = 1.0 - uv.y;
				uv.y = frac(uv.y + _Time.y);
				fixed4 col = tex2D(_MainTex, uv);
				return col;
			}
			ENDCG
		}
	}
}
*/

Shader "Custom/CubemapShader" {
	Properties{
		_Cube("Cubemap", CUBE) = "" {}
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Lambert

			struct Input {
				float2 uv_MainTex;
				float3 worldRefl;
			};

			samplerCUBE _Cube;

		void surf(Input IN, inout SurfaceOutput o) {
			o.Emission = texCUBE(_Cube, IN.worldRefl).rgb;
		}
			ENDCG
	}
		FallBack "Diffuse"
}
